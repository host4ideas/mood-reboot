using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCenters
    {
        public Task<List<CenterListView>> GetAllCenters();
        public Task<List<CenterListView>> GetUserCentersAsync(int userId);
        public Task<Center?> FindCenter(int id);
        public Task CreateCenter(string email, string name, string address, string file, string telephone);
        public Task UpdateCenter(Center center);
        public Task DeleteCenter(int id);
    }
}
