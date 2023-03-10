using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCryptography.Helpers;

namespace MoodReboot.Repositories
{
    public class RepositoryUsersSql : IRepositoryUsers
    {
        private readonly MoodRebootContext context;

        public RepositoryUsersSql(MoodRebootContext context)
        {
            this.context = context;
        }

        public int GetMaximo()
        {
            if (!context.Users.Any())
            {
                return 1;
            }
            else
            {
                return this.context.Users.Max(z => z.Id) + 1;
            }
        }

        public Task<User?> FindUser(int userId)
        {
            return this.context.Users.FirstOrDefaultAsync(x => x.Id == userId);
        }

        public List<User> GetAllUsers()
        {
            return this.context.Users.ToList();
        }

        public async Task RegisterUser(string nombre, string firstName, string lastName, string email, string password, string image)
        {
            string salt = HelperCryptography.GenerateSalt();

            User user = new()
            {
                Id = this.GetMaximo(),
                UserName = nombre,
                LastName = lastName,
                FirstName = firstName,
                SignedDate = DateTime.UtcNow,
                Role = "USER",
                Email = email,
                Image = image,
                Salt = salt,
                Password = HelperCryptography.EncryptPassword(password, salt)
            };

            this.context.Users.Add(user);
            await this.context.SaveChangesAsync();
        }

        public async Task<User?> LoginUser(string email, string password)
        {
            User? user = await this.context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                // Recuperamos la password cifrada de la BBDD
                byte[] userPass = user.Password;
                string salt = user.Salt;
                byte[] temp = HelperCryptography.EncryptPassword(password, salt);
                if (HelperCryptography.CompareArrays(userPass, temp))
                {
                    return user;
                }
            }
            return default;
        }

        public async Task DeleteUser(int userId)
        {
            User? user = await this.FindUser(userId);
            if (user != null)
            {
                this.context.Users.Remove(user);
            }
        }
    }
}
