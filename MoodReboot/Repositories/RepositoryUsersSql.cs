using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCryptography.Helpers;

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
        public async Task<int> GetMaxUser()
        {
            if (!context.Users.Any())
            {
                return 1;
            }

            return await this.context.Users.MaxAsync(z => z.Id) + 1;
        }

        public Task<User?> FindUser(int userId)
        {
            return this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public List<User> GetAllUsers()
        {
            return this.context.Users.ToList();
        }

        public async Task RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image)
        {
            string salt = HelperCryptography.GenerateSalt();

            User user = new()
            {
                Id = await this.GetMaxUser(),
                UserName = nombre,
                LastName = lastName,
                FirstName = firstName,
                SignedDate = DateTime.UtcNow,
                Role = "USER",
                Email = email,
                Image = image,
                Salt = salt,
                Password = HelperCryptography.EncryptPassword(password, salt)
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<User?> LoginUser(string email, string password)
        {
            User? user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                // Recuperamos la password cifrada de la BBDD
                byte[] userPass = user.Password;
                string salt = user.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                if (HelperCryptography.CompareArrays(userPass, temp))
                {
                    return user;
                }
            }
            return default;
        }

        public async Task DeleteUser(int userId)
        {
            User? user = await this.FindUser(userId);
            if (user != null)
            {
                this.context.Users.Remove(user);
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
        private async Task<int> GetMaxMessage()
        {
            if (!this.context.Messages.Any())
            {
                return 1;
            }

            int max = await this.context.Messages.MaxAsync(x => x.MessageId) + 1;
            return max;
        }

        public async Task CreateMessage(int userId, int groupChatId, string userName, string? text = null, int? fileId = null, bool seen = false)
        {
            Message message = new()
            {
                MessageId = await this.GetMaxMessage(),
                UserID = userId,
                GroupId = groupChatId,
                Text = text,
                FileId = fileId,
                DatePosted = DateTime.UtcNow,
                UserName = userName,
                Seen = seen
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

        public async Task AddUsersToChat(List<int> userIds, int chatGroupId)
        {
            List<UserChatGroup> userChatGroups = new();

            int firstIndex = await this.GetMaxUserChatGroup();

            foreach (int userId in userIds)
            {
                UserChatGroup userChatGroup = new()
                {
                    Id = firstIndex,
                    UserID = userId,
                    GroupId = chatGroupId,
                    JoinDate = DateTime.Now
                };

                userChatGroups.Add(userChatGroup);
                firstIndex++;
            }

            await this.context.UserChatGroups.AddRangeAsync(userChatGroups);
            await this.context.SaveChangesAsync();
        }

        public List<Message> GetMessagesByGroup(int chatGroupId)
        {
            return this.context.Messages.Where(x => x.GroupId == chatGroupId).ToList();
        }

        public List<UserChatGroup> GetUserChatGroups(int userId)
        {
            return this.context.UserChatGroups.Where(x => x.UserID == userId).ToList();
        }
        #endregion
    }
}
