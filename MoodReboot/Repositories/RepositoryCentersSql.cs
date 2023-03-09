using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryCentersSql : IRepositoryCenters
    {
        private readonly MoodRebootContext _context;

        public RepositoryCentersSql(MoodRebootContext context)
        {
            this._context = context;
        }

        public Task CreateCenter(string email, string name, string address, string telephone, string image)
        {
            string sql = "SP_CREATE_CENTER @EMAIL, @NAME, @ADDRESS, @TELEPHONE, @IMAGE";

            SqlParameter[] sqlParameters = new[]
            {
                new SqlParameter("@EMAIL", email),
                new SqlParameter("@NAME", name),
                new SqlParameter("@ADDRESS", address),
                new SqlParameter("@TELEPHONE", telephone),
                new SqlParameter("@IMAGE", image),
            };

            return this._context.Database.ExecuteSqlRawAsync(sql, sqlParameters);
        }

        public async Task DeleteCenter(int id)
        {
            Center? center = await this._context.Centers.FirstOrDefaultAsync(x => x.Id == id);
            if (center != null)
            {
                await this._context.SaveChangesAsync();
            }
        }

        public async Task<Center?> FindCenter(int id)
        {
            return await this._context.Centers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<CenterListView> GetAllCenters()
        {
            List<Center> centers = this._context.Centers.ToList();

            var result = from c in _context.Centers
                         join u in _context.Users on c.Director equals u.Id
                         select new CenterListView
                         {
                             CenterName = c.Name,
                             Director = new Author() { Id = u.Id, Image = u.Image, UserName = u.UserName },
                             Email = c.Email,
                             Id = c.Id,
                             Image = c.Image,
                             Telephone = c.Telephone
                         };

            return result.ToList();
        }

        public List<Center> GetUserCenters(int id)
        {
            string sql = "SP_USER_CENTERS @USER_ID";

            SqlParameter[] sqlParameters = new[]
            {
                new SqlParameter("@USER_ID", id),
            };

            List<Center> centers = this._context.Centers.FromSqlRaw(sql, sqlParameters).ToList();
            return centers;
        }

        public Task UpdateCenter(Center center)
        {
            string sql = "SP_UPDATE_CENTER @CENTER_ID, @EMAIL, @NAME, @ADDRESS, @TELEPHONE, @IMAGE";

            SqlParameter[] sqlParameters = new[]
            {
                new SqlParameter("@CENTER_ID", center.Id),
                new SqlParameter("@EMAIL", center.Id),
                new SqlParameter("@NAME", center.Name),
                new SqlParameter("@ADDRESS", center.Address),
                new SqlParameter("@TELEPHONE", center.Telephone),
                new SqlParameter("@IMAGE", center.Image),
            };

            return this._context.Database.ExecuteSqlRawAsync(sql, sqlParameters);
        }
    }
}
