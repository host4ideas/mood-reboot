using NugetMoodReboot.Helpers;
using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiCenters : IRepositoryCenters
    {
        private readonly HelperApi helperApi;

        public ServiceApiCenters(HelperApi helperApi)
        {
            this.helperApi = helperApi;
        }

        public Task AddEditorsCenterAsync(int centerId, List<int> userIds)
        {
            string request = "/api/centers/addcenterseditors/";
        }

        public Task ApproveCenterAsync(Center center)
        {
            throw new NotImplementedException();
        }

        public Task CreateCenterAsync(string email, string name, string address, string telephone, string image, int director, bool approved)
        {
            throw new NotImplementedException();
        }

        public Task DeleteCenterAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Center?> FindCenterAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<CenterListView>> GetAllCentersAsync()
        {
            return this.helperApi.GetAsync<List<CenterListView>>("/api/centers/getcenters");
        }

        public Task<List<AppUser>> GetCenterEditorsAsync(int centerId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxCenterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Center>> GetPendingCentersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<CenterListView>> GetUserCentersAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserCenterAsync(int userId, int centerId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateCenterAsync(int centerId, string email, string name, string address, string telephone, string image)
        {
            throw new NotImplementedException();
        }
    }
}
