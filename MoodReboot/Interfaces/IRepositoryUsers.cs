using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryUsers
    {
        public List<User> GetUsers();
        public User FindUser(int id);
        public Task UpdateUser(User user);
        public Task DeleteUser(int id);
        public Task CreateUser(User user);
    }
}
