using APIMoodReboot.Utils;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiContents
    {
        private readonly HelperApi helperApi;
        private HttpContextAccessor httpContextAccessor;

        public ServiceApiContents(HelperApi helperApi, HttpContextAccessor httpContextAccessor)
        {
            this.helperApi = helperApi;
            this.httpContextAccessor = httpContextAccessor;
        }

        public async Task CreateContentAsync(int contentGroupId, string text)
        {
            CreateContentModelApi model = new()
            {
                GroupId = contentGroupId,
                UnsafeHtml = text,
            };

            await this.helperApi.PostAsync(Consts.ApiContent + "/addcontent/", model);
        }

        public async Task CreateContentFileAsync(int contentGroupId, int fileId)
        {
            // Find file in BBDD
            AppFile? file = await this.helperApi.GetAsync<AppFile>(Consts.ApiFiles + "/FindFile/" + fileId);

            if (file != null)
            {
                CreateContentModelApi model = new()
                {
                    GroupId = contentGroupId,
                    File = file
                };

                await this.helperApi.PostAsync(Consts.ApiContent + "/AddContent", model);
            }
        }

        public async Task DeleteContentAsync(int id)
        {
            await this.helperApi.DeleteAsync(Consts.ApiContent + "/DeleteContent/" + id);
        }

        public Task<Content?> FindContentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Content>> GetContentByGroupAsync(int groupId)
        {
            return this.helperApi.GetAsync<List<Content>>(Consts.ApiContent + "/GetContentByGroup/" + groupId);
        }

        public Task<int> GetMaxContentAsync()
        {
            return this.helperApi.GetAsync<int>(Consts.ApiContent + "/GetMaxContent/");
        }

        public async Task UpdateContentAsync(int id, string? text = null, int? fileId = null)
        {
            if (fileId != null)
            {
                // Find file in BBDD
                AppFile? file = await this.helperApi.GetAsync<AppFile>(Consts.ApiFiles + "/FindFile/" + fileId);

                UpdateContentApiModel model = new()
                {
                    ContentId = id,
                    File = file,
                };
                await this.helperApi.PutAsync(Consts.ApiContent + "/UpdateContent", model);
            }

            if (text != null)
            {
                UpdateContentApiModel model = new()
                {
                    ContentId = id,
                    UnsafeHtml = text,
                };
                await this.helperApi.PutAsync(Consts.ApiContent + "/UpdateContent", model);
            }
        }
    }
}
