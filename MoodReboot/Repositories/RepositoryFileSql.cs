using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryFileSql : IRepositoryFile
    {
        private readonly MoodRebootContext context;

        public RepositoryFileSql(MoodRebootContext context)
        {
            this.context = context;
        }

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
    }
}
