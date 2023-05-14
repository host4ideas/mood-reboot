using APIMoodReboot.Utils;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiContentGroups
    {
        private readonly HelperApi helperApi;

        public ServiceApiContentGroups(HelperApi helperApi)
        {
            this.helperApi = helperApi;
        }

        public async Task CreateContentGroupAsync(string name, int courseId, bool isVisible = false)
        {
            await this.helperApi.PostAsync(Consts.ApiContentGroups + $"/createcontentgroup/{name}/{courseId}/{isVisible}", null);
        }

        public async Task<List<ContentGroup>?> GetCourseContentGroupsAsync(int courseId)
        {
            return await this.helperApi.GetAsync<List<ContentGroup>>(Consts.ApiContentGroups + "/GetCourseContentGroups/" + courseId);
        }

        public async Task DeleteContentGroupAsync(int id)
        {
            await this.helperApi.DeleteAsync(Consts.ApiContentGroups + "/deletecontentgroup/" + id);
        }

        public Task<ContentGroup?> FindContentGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public async Task UpdateContentGroupAsync(int id, string name, bool isVisible)
        {
            await this.helperApi.PutAsync(Consts.ApiContentGroups + $"/updatecontentgroup/{id}/{name}/{isVisible}", null);
        }
    }
}
