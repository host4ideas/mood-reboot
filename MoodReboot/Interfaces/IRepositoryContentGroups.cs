using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryContentGroups
    {
        ContentGroup? FindContentGroup(int id);
        List<ContentGroup> GetCourseContentGroups(int courseId);
        Task UpdateContentGroupAsync(int id, string name, bool isVisible);
        Task CreateContentGroupAsync(string name, int courseId, bool isVisible = false);
        Task DeleteContentGroupAsync(int id);
    }
}
