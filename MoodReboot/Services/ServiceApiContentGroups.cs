using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiContentGroups : IRepositoryContentGroups
    {
        public Task CreateContentGroupAsync(string name, int courseId, bool isVisible = false)
        {
            throw new NotImplementedException();
        }

        public Task DeleteContentGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<ContentGroup?> FindContentGroupAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task UpdateContentGroupAsync(int id, string name, bool isVisible)
        {
            throw new NotImplementedException();
        }
    }
}
