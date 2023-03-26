using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        // Users
        Task<AppUser?> FindUser(int userId);
        public Task ApproveUser(AppUser user);
        public Task ApproveUser(int userId);
        public Task<List<AppUser>> GetPendingUsers();
        public Task<bool> IsEmailAvailable(string email);
        public Task<bool> IsUsernameAvailable(string userName);
        Task<List<Tuple<string, int>>> SearchUsers(string pattern);
        Task<int> GetMaxUser();
        List<AppUser> GetAllUsers();
        Task<AppUser?> LoginUser(string email, string password);
        Task<int> RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image);
        Task DeleteUser(int userId);
        public Task UpdateUserBasics(int userId, string userName, string firstName, string lastName, string? image = null);
        public Task UpdateUserEmail(int userId, string email);
        public Task UpdateUserPassword(int userId, string password);
        public Task DeactivateUser(int userId);
        // User Action
        public Task<UserAction?> FindUserAction(int userId, string token);
        public Task RemoveUserAction(UserAction userAction);
        public Task<string> CreateUserAction(int userId);
        // Files
        Task<AppFile?> FindFile(int fileId);
        Task<int> GetMaxFile();
        Task DeleteFile(int fileId);
        Task UpdateFileAsync(int fileId, string fileName, string mimeType);
        Task UpdateFileAsync(int fileId, string fileName, string mimeType, int userId);
        Task<int> InsertFileAsync(string name, string mimeType);
        Task<int> InsertFileAsync(string name, string mimeType, int userId);
        // Messages
        List<ChatGroup> GetUserChatGroups(int userId);
        List<Message> GetMessagesByGroup(int chatGroupId);
        Task CreateMessage(int userId, int groupChatId, string userName, string? text = null, int? fileId = null);
        Task DeleteMessage(int id);
        List<Message> GetUnseenMessages(int userId);
        Task UpdateChatLastSeen(int chatGroupId, int userId);
        public Task NewChatGroup(HashSet<int> userIdsNoDups);
        public Task<int> NewChatGroup(HashSet<int> userIdsNoDups, int adminUserId, string chatGroupName);
        public Task RemoveChatGroup(int chatGroupId);
        public Task UpdateChatGroup(int chatGroupId, string name);
        public Task<List<ChatUserModel>> GetChatGroupUsers(int chatGroupId);
        public Task RemoveChatUser(int userId, int chatGroupId);
        public Task AddUsersToChat(int chatGroupId, List<int> userIds);
    }
}
