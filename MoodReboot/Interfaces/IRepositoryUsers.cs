using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        // Users
        Task<User?> FindUser(int userId);
        Task<List<Tuple<string, int>>> SearchUsers(string pattern);
        Task<int> GetMaxUser();
        List<User> GetAllUsers();
        Task<User?> LoginUser(string email, string password);
        Task RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image);
        Task DeleteUser(int userId);
        public Task UpdateUserBasics(int userId, string userName, string firstName, string lastName, string? image = null);
        public Task UpdateUserEmail(int userId, string email);
        public Task UpdateUserPassword(int userId, string password);
        // Files
        Task DeleteFile(int fileId);
        Task<int> InsertFileAsync(string name, string mimeType);
        Task<int> InsertFileAsync(string name, string mimeType, int userId);
        // Messages
        List<ChatGroup> GetUserChatGroups(int userId);
        List<Message> GetMessagesByGroup(int chatGroupId);
        Task CreateMessage(int userId, int groupChatId, string userName, string? text = null, int? fileId = null);
        Task DeleteMessage(int id);
        List<Message> GetUnseenMessages(int userId);
        Task UpdateChatLastSeen(int chatGroupId, int userId);
        Task NewChatGroup(List<int> userIds, string chatGroupName = "PRIVATE");
    }
}
