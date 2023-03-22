using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryCentersSql : IRepositoryCenters
    {
        private readonly MoodRebootContext context;

        public RepositoryCentersSql(MoodRebootContext context)
        {
            this.context = context;
        }

        public Task<List<Center>> GetPendingCenters()
        {
            return this.context.Centers.Where(x => x.Approved == false).ToListAsync();
        }

        public async Task ApproveCenter(Center center)
        {
            center.Approved = true;
            await this.context.SaveChangesAsync();
        }

        private async Task<int> GetMaxCenter()
        {
            return await this.context.Centers.MaxAsync(x => x.Id) + 1;
        }

        public async Task CreateCenter(string email, string name, string address, string telephone, string image, int director, bool approved)
        {
            this.context.Centers.Add(new()
            {
                Id = await this.GetMaxCenter(),
                Name = name,
                Address = address,
                Telephone = telephone,
                Image = image,
                Director = director,
                Email = email,
                Approved = approved
            });
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteCenter(int id)
        {
            Center? center = await this.context.Centers.FirstOrDefaultAsync(x => x.Id == id);
            if (center != null)
            {
                await this.context.SaveChangesAsync();
            }
        }

        public async Task<Center?> FindCenter(int id)
        {
            return await this.context.Centers.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<CenterListView>> GetAllCenters()
        {
            List<Center> centers = this.context.Centers.ToList();

            var result = from c in context.Centers
                         join u in context.Users on c.Director equals u.Id
                         select new CenterListView
                         {
                             CenterName = c.Name,
                             Director = new Author() { Id = u.Id, Image = u.Image, UserName = u.UserName, Email = u.Email },
                             Email = c.Email,
                             Id = c.Id,
                             Address = c.Address,
                             Image = c.Image,
                             Telephone = c.Telephone
                         };

            return result.ToListAsync();
        }

        public Task<List<User>> GetCenterEditorsAsync(int centerId)
        {
            var result = from uc in this.context.UserCenters
                         join u in this.context.Users on uc.UserId equals u.Id
                         where uc.CenterId == centerId && uc.IsEditor == true
                         select u;
            return result.ToListAsync();
        }

        public async Task<List<CenterListView>> GetUserCentersAsync(int userId)
        {
            List<Center> centers = await this.context.Centers.ToListAsync();

            var result = from uc in this.context.UserCenters
                         join c in this.context.Centers on uc.CenterId equals c.Id
                         join u in this.context.Users on uc.UserId equals u.Id
                         where uc.UserId == userId
                         select new CenterListView
                         {
                             CenterName = c.Name,
                             Director = new Author() { Id = u.Id, Image = u.Image, UserName = u.UserName, Email = u.Email },
                             Email = c.Email,
                             Id = c.Id,
                             Address = c.Address,
                             Image = c.Image,
                             Telephone = c.Telephone,
                             IsEditor = uc.IsEditor
                         };

            return await result.ToListAsync();
        }

        public async Task UpdateCenter(int centerId, string email, string name, string address, string telephone, string image)
        {
            Center? center = await this.FindCenter(centerId);
            if (center != null)
            {
                center.Name = name;
                center.Address = address;
                center.Telephone = telephone;
                center.Image = image;
                center.Email = email;

                await this.context.SaveChangesAsync();
            }
        }
    }
}
