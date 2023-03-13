using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        // Users
        Task<int> GetMaxUser();
        Task<User?> FindUser(int userId);
        List<User> GetAllUsers();
        Task<User?> LoginUser(string email, string password);
        Task RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image);
        Task DeleteUser(int userId);
        // Files
        Task DeleteFile(int fileId);
        Task<int> InsertFileAsync(string name, string mimeType);
        Task<int> InsertFileAsync(string name, string mimeType, int userId);
        // Messages
        List<ChatGroup> GetUserChatGroups(int userId);
        List<Message> GetMessagesByGroup(int chatGroupId);
        Task CreateMessage(int userId, int groupChatId, string userName, string? text = null, int? fileId = null, bool seen = false);
        Task DeleteMessage(int id);
    }
}
