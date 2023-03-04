using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryContent
    {
        public Task<Content?> FindContent(int id);
        public List<Content> GetContentByGroup(int groupId);
        public List<ContentGroup> GetCourseContentGroups(int courseId);
        public Task UpdateContentGroup(int id, string name, Boolean isVisible);
        public Task DeleteContentGroup(int id);
        public Task CreateContentGroup(string name, int courseId, Boolean isVisible = false);
        public Task UpdateContent(int id, string? text = null, int? fileId = null);
        public Task CreateContent(int contentGroupId, string text);
        public Task CreateContentFile(int contentGroupId, int fileId);
        public Task DeleteContent(int id);
    }
}
