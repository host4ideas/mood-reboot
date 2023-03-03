using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryCoursesSql : IRepositoryCourses
    {
        private readonly MoodRebootContext context;

        public RepositoryCoursesSql(MoodRebootContext context)
        {
            this.context = context;
        }

        public Task CreateCourse(string name, string? description, string? image, int? isVisible)
        {
            string sql = "SP_CREATE_COURSE @NAME, @DESCRIPTION, @IMAGE, @IS_VISIBLE";

            SqlParameter[] sqlParameters = new[] {
                new SqlParameter("@NAME", name),
                new SqlParameter("@DESCRIPTION", description),
                new SqlParameter("@IMAGE", image),
                new SqlParameter("@IS_VISIBLE", isVisible),
            };

            return this.context.Database.ExecuteSqlRawAsync(sql, sqlParameters);
        }

        public async Task DeleteCourse(int id)
        {
            Course? course = await this.FindCourse(id);
            if (course != null)
            {
                this.context.Courses.Remove(course);
                await this.context.SaveChangesAsync();
            }
        }

        public Task<Course?> FindCourse(int id)
        {
            return this.context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Course> GetAllCourses()
        {
            return this.context.Courses.ToList();
        }

        public List<Course> GetUserCourses(int id)
        {
            string sql = "SP_USER_COURSES @USER_ID";

            SqlParameter[] sqlParameters = new[]
            {
                new SqlParameter("@USER_ID", id),
            };

            return this.context.Courses.FromSqlRaw(sql, sqlParameters).ToList();
        }

        public List<Course> GetCenterCourses(int id)
        {
            var consulta = from datos in this.context.Courses
                          where datos.CenterId == id
                          select datos;
            return consulta.ToList();
        }

        public async Task UpdateCourse(int id, string description, string image, string name, Boolean isVisible)
        {
            Course? course = await this.FindCourse(id);

            if (course != null)
            {
                course.Description = description;
                course.IsVisible = isVisible;
                course.Image = image;
                course.Name = name;
                await this.context.SaveChangesAsync();
            }
        }
    }
}
