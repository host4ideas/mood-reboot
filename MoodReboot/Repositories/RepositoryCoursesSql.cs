using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using System.Collections.Generic;

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
        /// Adds the authors to a given list of courses
        /// </summary>
        /// <param name="courseListView"></param>
        /// <returns></returns>
        public List<CourseListView> GetCoursesAuthors(List<CourseListView> courseListView)
        {
            List<int> courseIds = new();

            foreach (CourseListView course in courseListView)
            {
                courseIds.Add(course.CourseId);
            }

            var result2 = from u in context.Users
                          join uc in context.UserCourses on u.Id equals uc.UserId
                          where courseIds.Contains(uc.CourseId) && uc.IsEditor == true
                          select new { u.UserName, u.Image, u.Id, uc.CourseId };

            var possibleAuthors = result2.ToList();

            // Filter the authors for each course
            foreach (CourseListView course in courseListView)
            {
                var courseAuthors = possibleAuthors.Where(p => p.CourseId == course.CourseId);

                if (courseAuthors.Any())
                {
                    // Convert all anonynous objects of the result to Author Model
                    List<Author> authors = courseAuthors.ToList().ConvertAll(x => new Author() { Id = x.Id, Image = x.Image, UserName = x.UserName });
                    course.Authors = authors;
                }
            }

            return courseListView;
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

            // Courses without authors
            List<CourseListView> courseListView = result.ToList();

            // Courses with authors
            List<CourseListView> coursesAuthors = this.GetCoursesAuthors(courseListView);

            return coursesAuthors;
        }

        public List<CourseListView> CenterCoursesListView(int centerId)
        {
            // Courses without authors
            List<CourseListView> courseListView = this.GetCenterCourses(centerId);

            // Courses with authors
            List<CourseListView> coursesAuthors = this.GetCoursesAuthors(courseListView);

            return coursesAuthors;
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
