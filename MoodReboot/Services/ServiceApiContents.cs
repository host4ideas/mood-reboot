using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiContents : IRepositoryContent
    {
        public Task CreateContentAsync(int contentGroupId, string text)
        {
            throw new NotImplementedException();
        }

        public Task CreateContentFileAsync(int contentGroupId, int fileId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<Content?> FindContentAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<List<Content>> GetContentByGroupAsync(int groupId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxContentAsync()
        {
            throw new NotImplementedException();
        }

        public Task UpdateContentAsync(int id, string? text = null, int? fileId = null)
        {
            throw new NotImplementedException();
        }
    }
}
