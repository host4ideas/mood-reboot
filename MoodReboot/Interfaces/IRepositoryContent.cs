using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryContent
    {
        Task CreateContent(int contentGroupId, string text);
        Task CreateContentFile(int contentGroupId, int fileId);
        Task CreateContentGroup(string name, int courseId, bool isVisible = false);
        Task DeleteContent(int id);
        Task DeleteContentGroup(int id);
        Task<Content?> FindContent(int id);
        Task<ContentGroup?> FindContentGroup(int id);
        List<Content> GetContentByGroup(int groupId);
        List<ContentGroup> GetCourseContentGroups(int courseId);
        Task UpdateContent(int id, string? text = null, int? fileId = null);
        Task UpdateContentGroup(int id, string name, bool isVisible);
    }
}