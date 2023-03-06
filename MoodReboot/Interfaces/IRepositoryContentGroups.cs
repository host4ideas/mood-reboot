using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryContentGroups
    {
        ContentGroup? FindContentGroup(int id);
        Task UpdateContentGroup(int id, string name, bool isVisible);
        Task CreateContentGroup(string name, int courseId, bool isVisible = false);
        Task DeleteContentGroup(int id);
        List<ContentGroup> GetCourseContentGroups(int courseId);
    }
}
