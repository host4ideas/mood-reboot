using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCenters
    {
        public List<Center> GetAllCenters();
        public List<Center> GetUserCenters(int id);
        public Task<Center?> FindCenter(int id);
        public Task CreateCenter(string email, string name, string address, string file, string telephone);
        public Task UpdateCenter(Center center);
        public Task DeleteCenter(int id);
    }
}
