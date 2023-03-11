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
        public int GetMaximo()
        {
            if (!context.Users.Any())
            {
                return 1;
            }
            else
            {
                return this.context.Users.Max(z => z.Id) + 1;
            }
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
                Id = this.GetMaximo(),
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
        public Task CreateMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessage(int id)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessagesByGroup()
        {
            throw new NotImplementedException();
        }

        public List<int> GetUserChatGroups(int userId)
        {
            var result = from ug in this.context.UserChatGroups
                         where ug.UserID == userId
                         select ug.GroupId;
            return result.ToList();
        }
        #endregion
    }
}
