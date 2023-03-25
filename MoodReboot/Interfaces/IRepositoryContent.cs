using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryContent
    {
        public Task<int> GetMaxContent();
        Task CreateContent(int contentGroupId, string text);
        Task CreateContentFile(int contentGroupId, int fileId);
        Task DeleteContent(int id);
        Task<Content?> FindContent(int id);
        public Task<List<Content>> GetContentByGroup(int groupId);
        Task UpdateContent(int id, string? text = null, int? fileId = null);
    }
}
