using NugetMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace MoodReboot.Services
{
    public class ServiceApiUsers : IRepositoryUsers
    {
        public Task AddUsersToChatAsync(int chatGroupId, List<int> userIds)
        {
            throw new NotImplementedException();
        }

        public Task ApproveUserAsync(AppUser user)
        {
            throw new NotImplementedException();
        }

        public Task ApproveUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task CreateMessageAsync(int userId, int groupChatId, string userName, string? text = null, int? fileId = null)
        {
            throw new NotImplementedException();
        }

        public Task<string> CreateUserActionAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task DeactivateUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteFileAsync(int fileId)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessageAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task DeleteUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<AppFile?> FindFileAsync(int fileId)
        {
            throw new NotImplementedException();
        }

        public Task<UserAction?> FindUserActionAsync(int userId, string token)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser?> FindUserAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppUser>> GetAllUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<ChatUserModel>> GetChatGroupUsersAsync(int chatGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxFileAsync()
        {
            throw new NotImplementedException();
        }

        public Task<int> GetMaxUserAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetMessagesByGroupAsync(int chatGroupId)
        {
            throw new NotImplementedException();
        }

        public Task<List<AppUser>> GetPendingUsersAsync()
        {
            throw new NotImplementedException();
        }

        public Task<List<Message>> GetUnseenMessagesAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<List<ChatGroup>> GetUserChatGroupsAsync(int userId)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertFileAsync(string name, string mimeType)
        {
            throw new NotImplementedException();
        }

        public Task<int> InsertFileAsync(string name, string mimeType, int userId)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsEmailAvailableAsync(string email)
        {
            throw new NotImplementedException();
        }

        public Task<bool> IsUsernameAvailableAsync(string userName)
        {
            throw new NotImplementedException();
        }

        public Task<AppUser?> LoginUserAsync(string email, string password)
        {
            throw new NotImplementedException();
        }

        public Task NewChatGroupAsync(HashSet<int> userIdsNoDups)
        {
            throw new NotImplementedException();
        }

        public Task<int> NewChatGroupAsync(HashSet<int> userIdsNoDups, int adminUserId, string chatGroupName)
        {
            throw new NotImplementedException();
        }

        public Task<int> RegisterUserAsync(string nombre, string firstName, string lastName, string email, string password, string image)
        {
            throw new NotImplementedException();
        }

        public Task RemoveChatGroupAsync(int chatGroupId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveChatUserAsync(int userId, int chatGroupId)
        {
            throw new NotImplementedException();
        }

        public Task RemoveUserActionAsync(UserAction userAction)
        {
            throw new NotImplementedException();
        }

        public Task<List<Tuple<string, int>>> SearchUsersAsync(string pattern)
        {
            throw new NotImplementedException();
        }

        public Task UpdateChatGroupAsync(int chatGroupId, string name)
        {
            throw new NotImplementedException();
        }

        public Task UpdateChatLastSeenAsync(int chatGroupId, int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFileAsync(int fileId, string fileName, string mimeType)
        {
            throw new NotImplementedException();
        }

        public Task UpdateFileAsync(int fileId, string fileName, string mimeType, int userId)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserBasicsAsync(int userId, string userName, string firstName, string lastName, string? image = null)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserEmailAsync(int userId, string email)
        {
            throw new NotImplementedException();
        }

        public Task UpdateUserPasswordAsync(int userId, string password)
        {
            throw new NotImplementedException();
        }
    }
}
