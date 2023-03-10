using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        int GetMaximo();
        public Task<User?> FindUser(int userId);
        List<User> GetAllUsers();
        Task<User?> LoginUser(string email, string password);
        Task RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image);
        public Task DeleteUser(int userId);
    }
}
