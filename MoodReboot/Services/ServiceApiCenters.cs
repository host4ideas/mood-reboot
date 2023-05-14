using APIMoodReboot.Utils;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiCenters
    {
        private readonly HelperApi helperApi;
        private readonly HttpContextAccessor httpContextAccessor;

        public ServiceApiCenters(HelperApi helperApi, HttpContextAccessor httpContextAccessor)
        {
            this.helperApi = helperApi;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task<bool> CreateCourseAsync(int centerId, string name, bool isVisible, string image, string description, string password)
        {
            CreateCourseApiModel model = new()
            {
                CenterId = centerId,
                Description = description,
                Image = image,
                IsVisible = isVisible,
                Name = name,
                Password = password
            };

            string token = this.httpContextAccessor.HttpContext.Session.GetString("TOKEN");
            var response = await this.helperApi.PostAsync(Consts.ApiCourses + "/", model, token);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }

            return false;
        }

        public async Task AddEditorsCenterAsync(int centerId, List<int> userIds)
        {
            string request = Consts.ApiCenters + "/addcenterseditors/";

            AddCenterEditorsApiModel model = new()
            {
                CenterId = centerId,
                UserIds = userIds
            };

            string token = this.httpContextAccessor.HttpContext.Session.GetString("TOKEN");
            await this.helperApi.PostAsync(request, model, token);
        }

        public async Task ApproveCenterAsync(Center center)
        {
            string request = Consts.ApiAdmin + "/approvecenter/" + center.Id;
            await this.helperApi.PutAsync(request, null);
        }

        public async Task CreateCenterAsync(string email, string name, string address, string telephone, string image, int director, bool approved)
        {
            string request = Consts.ApiAdmin + "/centerrequest";

            Center center = new()
            {
                Address = address,
                Approved = approved,
                Director = director,
                Email = email,
                Name = name,
                Image = image,
                Telephone = telephone,
            };

            await this.helperApi.PostAsync(request, center);
        }

        public async Task DeleteCenterAsync(int id)
        {
            await this.helperApi.DeleteAsync(Consts.ApiCenters + "/DeleteCenter/" + id);
        }

        public Task<Center?> FindCenterAsync(int id)
        {
            return this.helperApi.GetAsync<Center>(Consts.ApiCenters + "/");
        }

        public Task<List<CenterListView>?> GetAllCentersAsync()
        {
            return this.helperApi.GetAsync<List<CenterListView>?>(Consts.ApiCenters + "/getcenters");
        }

        public Task<List<AppUser>?> GetCenterEditorsAsync(int centerId)
        {
            return this.helperApi.GetAsync<List<AppUser>?>(Consts.ApiCenters + "/");
        }

        public Task<int> GetMaxCenterAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Center>?> GetPendingCentersAsync()
        {
            return this.helperApi.GetAsync<List<Center>?>(Consts.ApiAdmin + "/centerrequests");
        }

        public Task<List<CenterListView>?> GetUserCentersAsync(int userId)
        {
            return this.helperApi.GetAsync<List<CenterListView>?>(Consts.ApiCenters + "/usercenters");
        }

        public Task RemoveUserCenterAsync(int userId, int centerId)
        {
            return this.helperApi.DeleteAsync(Consts.ApiCenters + $"/{userId}/{centerId}");
        }

        public Task UpdateCenterAsync(int centerId, string email, string name, string address, string telephone, string image)
        {
            throw new NotImplementedException();
        }
    }
}
