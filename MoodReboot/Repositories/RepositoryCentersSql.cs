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

        public async Task RemoveUserCenter(int userId, int centerId)
        {
            UserCenter userCenter = await this.context.UserCenters.FirstOrDefaultAsync(x => x.UserId == userId && x.CenterId == centerId);
            if (userCenter != null)
            {
                this.context.UserCenters.Remove(userCenter);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task AddEditorsCenter(int centerId, List<int> userIds)
        {
            int firstNewId = await this.GetMaxUserCenter();
            foreach (int userId in userIds)
            {
                UserCenter userCenter = new()
                {
                    Id = firstNewId,
                    CenterId = centerId,
                    UserId = userId,
                    IsEditor = true
                };
                this.context.UserCenters.Add(userCenter);
                firstNewId += 1;
            }
            await this.context.SaveChangesAsync();
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

        public async Task<int> GetMaxCenter()
        {
            if (!context.Centers.Any())
            {
                return 1;
            }

            return await this.context.Centers.MaxAsync(x => x.Id) + 1;
        }

        private async Task<int> GetMaxUserCenter()
        {
            if (!context.Centers.Any())
            {
                return 1;
            }

            return await this.context.UserCenters.MaxAsync(x => x.Id) + 1;
        }

        public async Task AddUserCenter(int userId, int centerId, bool isEditor)
        {
            UserCenter userCenter = new() { Id = await this.GetMaxUserCenter(), UserId = userId, CenterId = centerId, IsEditor = isEditor };
            await this.context.UserCenters.AddAsync(userCenter);
            await this.context.SaveChangesAsync();
        }

        public async Task CreateCenter(string email, string name, string address, string telephone, string image, int director, bool approved)
        {
            int centerId = await this.GetMaxCenter();

            await this.context.Centers.AddAsync(new()
            {
                Id = centerId,
                Name = name,
                Address = address,
                Telephone = telephone,
                Image = image,
                Director = director,
                Email = email,
                Approved = approved
            });

            await this.AddUserCenter(director, centerId, true);

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
        
        public async Task<List<CenterListView>> GetAllCenters()
        {
            //List<Center> centers = await this.context.Centers.ToListAsync();

            var result = from c in context.Centers
                         join u in context.Users on c.Director equals u.Id
                         where c.Approved == true
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

            return await result.ToListAsync();
        }

        public Task<List<AppUser>> GetCenterEditorsAsync(int centerId)
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
                         where uc.UserId == userId && c.Approved == true
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
