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

        public List<CourseUsersModel> GetCourseUsers(int courseId)
        {
            var result = from uc in this.context.UserCourses
                         join u in this.context.Users on uc.UserId equals u.Id
                         where uc.CourseId == courseId
                         select new CourseUsersModel
                         {
                             Id = u.Id,
                             UserName = u.UserName,
                             Image = u.Image,
                             IsEditor = uc.IsEditor
                         };

            return result.ToList();
        }

        public async Task RemoveCourseUserAsync(int courseId, int userId)
        {
            UserCourse? userCourse = await this.context.UserCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);

            if (userCourse != null)
            {
                this.context.UserCourses.Remove(userCourse);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task RemoveCourseEditorAsync(int courseId, int userId)
        {
            UserCourse? userCourse = await this.context.UserCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);

            if (userCourse != null)
            {
                userCourse.IsEditor = false;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task AddCourseEditorAsync(int courseId, int userId)
        {
            UserCourse? userCourse = await this.context.UserCourses.FirstOrDefaultAsync(x => x.UserId == userId && x.CourseId == courseId);

            if (userCourse != null)
            {
                userCourse.IsEditor = true;
                await this.context.SaveChangesAsync();
            }
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

        /// <summary>
        /// Get a user's courses
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public List<CourseListView> GetUserCourses(int userId)
        {
            var result = from c in context.Courses
                         join uc in context.UserCourses on c.Id equals uc.CourseId
                         join u in context.Users on uc.UserId equals u.Id
                         join ct in context.Centers on c.CenterId equals ct.Id
                         where uc.UserId == userId
                         select new CourseListView
                         {
                             CourseId = c.Id,
                             DatePublished = c.DatePublished,
                             DateModified = c.DateModified,
                             Description = c.Description,
                             Image = c.Image,
                             CourseName = c.Name,
                             CenterName = ct.Name,
                             IsEditor = uc.IsEditor
                         };

            var courseListView = result.ToList();

            foreach (CourseListView courseView in courseListView)
            {
                var result2 = from u in context.Users
                              join uc in context.UserCourses on u.Id equals uc.UserId
                              where uc.IsEditor == true && uc.CourseId == courseView.CourseId
                              select new Author { UserName = u.UserName, Image = u.Image, Id = u.Id };

                courseView.Authors = result2.ToList();
            }

            return courseListView;
        }

        public List<CourseListView> CenterCoursesListView(int centerId)
        {

            //var consulta = from datos in this._context.Empleados
            //               where ids.Contains(datos.IdEmpleado)
            //               select datos;

            //return consulta.ToList();

            var courses = this.GetCenterCourses(centerId);


            List<int> courseIds = new();

            foreach (var course in courses)
            {
                courseIds.Add(course.CourseId);

                //List <CourseUsersModel> users = this.GetCourseUsers(course.CourseId);
                //foreach (var user in users)
                //{
                //    if (user.IsEditor == true)
                //    {
                //        course.Authors?.Add(new Author { UserName = user.UserName, Image = user.Image });
                //    }
                //}
            }

            var consulta = from datos in this.context.Courses
                           where courseIds.Contains(datos.Id)
                           select datos;



            return courses;
        }

        /// <summary>
        /// Get a center's courses
        /// </summary>
        /// <param name="centerId"></param>
        /// <returns></returns>
        public List<CourseListView> GetCenterCourses(int centerId)
        {
            var result = from cr in this.context.Courses
                         join ct in this.context.Centers on cr.CenterId equals ct.Id
                         where ct.Id == centerId
                         select new CourseListView
                         {
                             CourseId = cr.Id,
                             CourseName = cr.Name,
                             DatePublished = cr.DatePublished,
                             DateModified = cr.DateModified,
                             Description = cr.Description,
                             Image = cr.Image,
                             CenterName = ct.Name,
                             Authors = new List<Author>()
                         };

            var courses = result.ToList();
            return courses;
        }

        public async Task UpdateCourse(int id, string description, string image, string name, bool isVisible)
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
