using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryCourses : IRepositoryCourses
    {
        private readonly MoodRebootSqlContext _context;

        public RepositoryCourses(MoodRebootSqlContext context)
        {
            _context = context;
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

            return this._context.Database.ExecuteSqlRawAsync(sql, sqlParameters);
        }

        public async Task DeleteCourse(int id)
        {
            Course? course = await this.FindCourse(id);
            if (course != null)
            {
                this._context.Courses.Remove(course);
                await this._context.SaveChangesAsync();
            }
        }

        public Task<Course?> FindCourse(int id)
        {
            return this._context.Courses.FirstOrDefaultAsync(x => x.Id == id);
        }

        public List<Course> GetAllCourses()
        {
            return this._context.Courses.ToList();
        }

        public List<Course> GetUserCourses(int id)
        {
            string sql = "SP_USER_COURSES @USER_ID";

            SqlParameter[] sqlParameters = new[]
            {
                new SqlParameter("@USER_ID", id),
            };

            return this._context.Courses.FromSqlRaw(sql, sqlParameters).ToList();
        }

        public Task UpdateCourse(Course course)
        {
            string sql = "SP_UPDATE_COURSE @COURSE_ID, @NAME, @DESCRIPTION, @IMAGE, @IS_VISIBLE";

            SqlParameter[] sqlParameters = new[]
            {
                new SqlParameter("@COURSE_ID", course.Id),
                new SqlParameter("@NAME", course.Name),
                new SqlParameter("@DESCRIPTION", course.Description),
                new SqlParameter("@IMAGE", course.Image),
                new SqlParameter("@IS_VISIBLE", course.IsVisible),
            };

            return this._context.Database.ExecuteSqlRawAsync(sql, sqlParameters);
        }
    }
}
