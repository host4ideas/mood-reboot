using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Models;
using MvcCryptography.Helpers;

namespace MoodReboot.Repositories
{
    public class RepositoryUsersSql
    {
        private MoodRebootContext context;

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

        public Task RegisterUser(string nombre, string email, string password, string imagen)
        {
            string salt = HelperCryptography.GenerateSalt();

            User user = new()
            {
                IdUser = this.GetMaximo(),
                Nombre = nombre,
                Email = email,
                Imagen = imagen,
                Salt = salt,
                Password = HelperCryptography.EncryptPassword(password, salt)
            };

            this.context.Users.Add(user);
            return this.context.SaveChangesAsync();
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
    }
}
