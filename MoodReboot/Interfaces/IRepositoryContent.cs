using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryContent
    {
        Task CreateContent(int contentGroupId, string text);
        Task CreateContentFile(int contentGroupId, int fileId);
        Task DeleteContent(int id);
        Content? FindContent(int id);
        List<Content> GetContentByGroup(int groupId);
        Task UpdateContent(int id, string? text = null, int? fileId = null);
    }
}