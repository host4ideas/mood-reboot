using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        // Users
        int GetMaximo();
        Task<User?> FindUser(int userId);
        List<User> GetAllUsers();
        Task<User?> LoginUser(string email, string password);
        Task RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image);
        Task DeleteUser(int userId);
        List<int> GetUserChatGroups(int userId);
        // Files
        Task DeleteFile(int fileId);
        Task<int> InsertFileAsync(string name, string mimeType);
        Task<int> InsertFileAsync(string name, string mimeType, int userId);
        // Messages
        List<Message> GetMessagesByGroup();
        Task CreateMessage(Message message);
        Task DeleteMessage(int id);
    }
}
