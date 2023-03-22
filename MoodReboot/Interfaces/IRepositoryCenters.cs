using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryCenters
    {
        public Task<List<CenterListView>> GetAllCenters();
        public Task<List<Center>> GetPendingCenters();
        public Task ApproveCenter(Center center);
        public Task<List<CenterListView>> GetUserCentersAsync(int userId);
        public Task<Center?> FindCenter(int id);
        public Task CreateCenter(string email, string name, string address, string telephone, string image, int director, bool approved);
        public Task UpdateCenter(Center center);
        public Task DeleteCenter(int id);
    }
}
