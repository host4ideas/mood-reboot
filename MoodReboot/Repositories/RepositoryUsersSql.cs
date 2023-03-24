using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryUsersSql : IRepositoryUsers
    {
        private readonly MoodRebootContext context;

        public RepositoryUsersSql(MoodRebootContext context)
        {
            this.context = context;
        }

        #region USERS

        private async Task<int> GetMaxUserAction()
        {
            if (!context.UserActions.Any())
            {
                return 1;
            }

            return await this.context.UserActions.MaxAsync(x => x.Id) + 1;
        }

        public async Task<string> CreateUserAction(int userId)
        {
            UserAction userAction = new()
            {
                Id = await this.GetMaxUserAction(),
                Token = Guid.NewGuid().ToString(),
                UserId = userId,
                RequestDate = DateTime.Now
            };

            await this.context.UserActions.AddAsync(userAction);
            await this.context.SaveChangesAsync();

            return userAction.Token;
        }

        public Task<UserAction?> FindUserAction(int userId, string token)
        {
            return this.context.UserActions.FirstOrDefaultAsync(x => x.UserId == userId && x.Token == token);
        }

        public async Task RemoveUserAction(UserAction userAction)
        {
            this.context.UserActions.Remove(userAction);
            await this.context.SaveChangesAsync();
        }

        public async Task ApproveUser(AppUser user)
        {
            user.Approved = true;
            await this.context.SaveChangesAsync();
        }

        public async Task ApproveUser(int userId)
        {
            AppUser? user = await this.FindUser(userId);
            if (user != null)
            {
                user.Approved = true;
                await this.context.SaveChangesAsync();
            }
        }

        public Task<List<AppUser>> GetPendingUsers()
        {
            return this.context.Users.Where(x => x.Approved == false).ToListAsync();
        }

        public async Task<bool> IsEmailAvailable(string email)
        {
            int count = await this.context.Users.CountAsync(u => u.Email == email);
            if (count > 0)
            {
                return false;
            }
            return true;
        }

        public async Task<bool> IsUsernameAvailable(string userName)
        {
            int count = await this.context.Users.CountAsync(u => u.UserName == userName);
            if (count > 0)
            {
                return false;
            }
            return true;
        }

        public Task<List<Tuple<string, int>>> SearchUsers(string pattern)
        {
            var result = (from u in this.context.Users
                          where u.UserName.Contains(pattern) || u.Email.Contains(pattern)
                          select new Tuple<string, int>(u.UserName, u.Id)).Take(10);
            return result.ToListAsync();
        }

        public async Task<int> GetMaxUser()
        {
            if (!context.Users.Any())
            {
                return 1;
            }

            return await this.context.Users.MaxAsync(z => z.Id) + 1;
        }

        public async Task<AppUser?> FindUser(int userId)
        {
            return await this.context.Users.FindAsync(userId);
        }

        public List<AppUser> GetAllUsers()
        {
            return this.context.Users.ToList();
        }

        public async Task<int> RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image)
        {
            string salt = HelperCryptography.GenerateSalt();

            int userId = await this.GetMaxUser();

            AppUser user = new()
            {
                Id = userId,
                UserName = nombre,
                LastName = lastName,
                FirstName = firstName,
                SignedDate = DateTime.UtcNow,
                Role = "USER",
                Email = email,
                Image = image,
                Salt = salt,
                Password = HelperCryptography.EncryptPassword(password, salt),
                Approved = false,
                PassTest = password
            };

            await this.context.Users.AddAsync(user);
            await this.context.SaveChangesAsync();

            return userId;
        }

        public async Task<AppUser?> LoginUser(string usernameOrEmail, string password)
        {
            AppUser? user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == usernameOrEmail || u.UserName == usernameOrEmail);
            if (user != null)
            {
                if (password == user.PassTest)
                {
                    user.LastSeen = DateTime.Now;
                    await this.context.SaveChangesAsync();
                    return user;
                }

                // Recuperamos la password cifrada de la BBDD
                //byte[] userPass = user.Password;
                //string salt = user.Salt;
                //byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                //if (HelperCryptography.CompareArrays(userPass, temp))
                //{
                //    user.LastSeen = DateTime.Now;
                //    await this.context.SaveChangesAsync();
                //    return user;
                //}
            }
            return default;
        }

        public async Task DeleteUser(int userId)
        {
            AppUser? user = await this.FindUser(userId);
            if (user != null)
            {
                this.context.Users.Remove(user);
            }
        }

        public async Task UpdateUserBasics(int userId, string userName, string firstName, string lastName, string? image = null)
        {
            AppUser? user = await this.FindUser(userId);
            if (user != null)
            {
                user.UserName = userName;
                user.FirstName = firstName;
                user.LastName = lastName;
                user.Image = image;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserEmail(int userId, string email)
        {
            AppUser? user = await this.FindUser(userId);
            if (user != null)
            {
                user.Email = email;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task UpdateUserPassword(int userId, string password)
        {
            string salt = HelperCryptography.GenerateSalt();

            AppUser? user = await this.FindUser(userId);

            if (user != null)
            {
                user.Password = HelperCryptography.EncryptPassword(password, salt);
                user.Salt = salt;
                user.PassTest = password;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeactivateUser(int userId)
        {
            AppUser? user = await this.FindUser(userId);
            if (user != null)
            {
                user.Approved = false;
                await this.context.SaveChangesAsync();
            }
        }

        #endregion

        #region FILES

        /// <summary>
        /// Inserts a new File in the File table of the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mimeType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> InsertFileAsync(string name, string mimeType)
        {
            string sql = "SP_CREATE_FILE @NAME, @MIME_TYPE, @USER_ID, @FILE_ID OUT";

            SqlParameter paramName = new("@NAME", name);
            SqlParameter paramMime = new("@MIME_TYPE", mimeType);
            SqlParameter paramFileIdOut = new("@FILE_ID", null)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await this.context.Database.ExecuteSqlRawAsync(sql, paramName, paramMime, paramFileIdOut);

            return (int)paramFileIdOut.Value;
        }

        /// <summary>
        /// Inserts a new File in the File table of the database
        /// </summary>
        /// <param name="name"></param>
        /// <param name="mimeType"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<int> InsertFileAsync(string name, string mimeType, int userId)
        {
            string sql = "SP_CREATE_FILE @NAME, @MIME_TYPE, @USER_ID, @FILE_ID OUT";

            SqlParameter paramName = new("@NAME", name);
            SqlParameter paramMime = new("@MIME_TYPE", mimeType);
            SqlParameter paramUserId = new("@USER_ID", userId);
            SqlParameter paramFileIdOut = new("@FILE_ID", null)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            await this.context.Database.ExecuteSqlRawAsync(sql, paramName, paramMime, paramUserId, paramFileIdOut);

            return (int)paramFileIdOut.Value;
        }

        /// <summary>
        /// Deletes a file from the File table of the database
        /// </summary>
        /// <param name="fileId"></param>
        /// <returns></returns>
        public async Task DeleteFile(int fileId)
        {
            AppFile? file = await this.context.Files.FirstOrDefaultAsync(x => x.Id == fileId);

            if (file != null)
            {
                this.context.Files.Remove(file);
                await this.context.SaveChangesAsync();
            }
        }

        #endregion

        #region MESSAGES

        public async Task AddUsersToChat(int chatGroupId, List<int> userIds)
        {
            // Create the chat group
            ChatGroup? chatGroup = await this.context.ChatGroups.FirstOrDefaultAsync(x => x.Id == chatGroupId);

            if (chatGroup != null)
            {
                List<ChatUserModel> users = await this.GetChatGroupUsers(chatGroupId);
                List<int> alreadyUserIds = users.ConvertAll(x => x.UserID).ToList();

                var consulta = from u in this.context.Users
                               join uc in this.context.UserChatGroups on u.Id equals uc.UserID
                               where userIds.Contains(u.Id)
                               select u.Id;

                List<int> newUserIds = await consulta.ToListAsync();

                alreadyUserIds.AddRange(newUserIds);

                HashSet<int> userIdsNoDups = new(alreadyUserIds);

                int firstId = await this.GetMaxUserChatGroup();

                // Add users to the chat group
                foreach (int userId in userIdsNoDups)
                {
                    UserChatGroup userChatGroup = new()
                    {
                        Id = firstId,
                        GroupId = chatGroup.Id,
                        JoinDate = DateTime.Now,
                        LastSeen = DateTime.Now,
                        UserID = userId,
                        IsAdmin = false
                    };

                    await this.context.UserChatGroups.AddAsync(userChatGroup);
                    firstId += 1;
                }

                await this.context.SaveChangesAsync();
            }
        }

        public async Task NewChatGroup(HashSet<int> userIdsNoDups)
        {
            // Create the chat group
            ChatGroup chatGroup = new()
            {
                Id = await this.GetMaxChatGroup(),
                Name = "PRIVATE",
            };

            await this.context.ChatGroups.AddAsync(chatGroup);

            int firstId = await this.GetMaxUserChatGroup();

            // Add users to the chat group
            foreach (int userId in userIdsNoDups)
            {
                UserChatGroup userChatGroup = new()
                {
                    Id = firstId,
                    GroupId = chatGroup.Id,
                    JoinDate = DateTime.Now,
                    LastSeen = DateTime.Now,
                    UserID = userId,
                    IsAdmin = false
                };

                await this.context.UserChatGroups.AddAsync(userChatGroup);
                firstId += 1;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task NewChatGroup(HashSet<int> userIdsNoDups, int adminUserId, string chatGroupName)
        {
            // Create the chat group
            ChatGroup chatGroup = new()
            {
                Id = await this.GetMaxChatGroup(),
                Name = chatGroupName,
            };

            await this.context.ChatGroups.AddAsync(chatGroup);

            int firstId = await this.GetMaxUserChatGroup();

            // Add users to the chat group
            foreach (int userId in userIdsNoDups)
            {
                bool isAdmin = false;

                if (adminUserId == userId)
                {
                    isAdmin = true;
                }

                UserChatGroup userChatGroup = new()
                {
                    Id = firstId,
                    GroupId = chatGroup.Id,
                    JoinDate = DateTime.Now,
                    LastSeen = DateTime.Now,
                    UserID = userId,
                    IsAdmin = isAdmin
                };

                await this.context.UserChatGroups.AddAsync(userChatGroup);
                firstId++;
            }

            await this.context.SaveChangesAsync();
        }

        public async Task UpdateChatGroup(int chatGroupId, string name)
        {
            ChatGroup? chatGroup = await this.context.ChatGroups.FirstOrDefaultAsync(x => x.Id == chatGroupId);
            if (chatGroup != null)
            {
                chatGroup.Name = name;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task RemoveChatGroup(int chatGroupId)
        {
            ChatGroup? chatGroup = await this.context.ChatGroups.FirstOrDefaultAsync(x => x.Id == chatGroupId);
            if (chatGroup != null)
            {
                this.context.ChatGroups.Remove(chatGroup);
                await this.context.SaveChangesAsync();
            }
        }

        private async Task<int> GetMaxMessage()
        {
            if (!this.context.Messages.Any())
            {
                return 1;
            }

            int max = await this.context.Messages.MaxAsync(x => x.MessageId) + 1;
            return max;
        }

        public async Task CreateMessage(int userId, int groupChatId, string userName, string? text = null, int? fileId = null)
        {
            Message message = new()
            {
                MessageId = await this.GetMaxMessage(),
                UserID = userId,
                GroupId = groupChatId,
                Text = text,
                FileId = fileId,
                DatePosted = DateTime.Now,
                UserName = userName
            };

            this.context.Messages.Add(message);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteMessage(int messageId)
        {
            Message? message = await this.context.Messages.FirstOrDefaultAsync(x => x.MessageId == messageId);

            if (message != null)
            {
                this.context.Messages.Remove(message);
                await this.context.SaveChangesAsync();
            }
        }

        private async Task<int> GetMaxChatGroup()
        {
            if (!this.context.ChatGroups.Any())
            {
                return 1;
            }

            int max = await this.context.ChatGroups.MaxAsync(x => x.Id);
            return max + 1;
        }

        public async Task CreateChat(string name, string? image)
        {
            ChatGroup chatGroup = new()
            {
                Id = await this.GetMaxChatGroup(),
                Name = name,
                Image = image
            };

            this.context.ChatGroups.Add(chatGroup);
            await this.context.SaveChangesAsync();
        }

        private async Task<int> GetMaxUserChatGroup()
        {
            if (!this.context.UserChatGroups.Any())
            {
                return 1;
            }

            int max = await this.context.UserChatGroups.MaxAsync(x => x.Id);
            return max + 1;
        }

        public async Task AddUsersToChat(HashSet<int> userIds, int chatGroupId)
        {
            List<UserChatGroup> userChatGroups = new();

            HashSet<int> usersNoDups = userIds;

            int firstIndex = await this.GetMaxUserChatGroup();

            foreach (int userId in usersNoDups)
            {
                userChatGroups.Add(new()
                {
                    Id = firstIndex,
                    UserID = userId,
                    GroupId = chatGroupId,
                    JoinDate = DateTime.Now
                });
                firstIndex++;
            }

            await this.context.UserChatGroups.AddRangeAsync(userChatGroups);
            await this.context.SaveChangesAsync();
        }

        public List<Message> GetMessagesByGroup(int chatGroupId)
        {
            return this.context.Messages.Where(x => x.GroupId == chatGroupId).ToList();
        }

        public List<ChatGroup> GetUserChatGroups(int userId)
        {
            var result = from g in context.ChatGroups
                         join ug in context.UserChatGroups on g.Id equals ug.GroupId
                         where ug.UserID == userId
                         select g;

            return result.ToList();
        }

        public List<Message> GetUnseenMessages(int userId)
        {
            var result = from ug in context.UserChatGroups
                         join m in context.Messages on ug.GroupId equals m.GroupId
                         where ug.UserID == userId && ug.LastSeen < m.DatePosted
                         select m;

            return result.ToList();
        }

        public async Task UpdateChatLastSeen(int chatGroupId, int userId)
        {
            UserChatGroup? userChatGroup = await this.context.UserChatGroups.FirstOrDefaultAsync(x => x.UserID == userId && x.GroupId == chatGroupId);
            if (userChatGroup != null)
            {
                userChatGroup.LastSeen = DateTime.Now;
                await this.context.SaveChangesAsync();
            }
        }

        public Task<List<ChatUserModel>> GetChatGroupUsers(int chatGroupId)
        {
            var result = from u in this.context.Users
                         join ug in this.context.UserChatGroups on u.Id equals ug.UserID
                         where ug.GroupId == chatGroupId
                         select new ChatUserModel()
                         {
                             IsAdmin = ug.IsAdmin,
                             UserID = u.Id,
                             UserName = u.UserName
                         };
            return result.ToListAsync();
        }

        public async Task RemoveChatUser(int userId, int chatGroupId)
        {
            UserChatGroup? userChat = await this.context.UserChatGroups.FirstOrDefaultAsync(x => x.GroupId == chatGroupId && x.UserID == userId);

            if (userChat != null)
            {
                this.context.UserChatGroups.Remove(userChat);
                await this.context.SaveChangesAsync();
            }
        }

        #endregion
    }
}
