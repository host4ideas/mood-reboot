using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        public int GetMaximo();
        public Task RegisterUser(string nombre, string email, string password, string imagen);
        public Task<User?> LoginUser(string email, string password);
        public List<User> GetUsers();
        public User FindUser(int id);
        public Task UpdateUser(User user);
        public Task DeleteUser(int id);
        public Task CreateUser(User user);
    }
}
