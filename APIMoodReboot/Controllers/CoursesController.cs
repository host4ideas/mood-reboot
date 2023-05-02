using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using System.Security.Claims;
using NugetMoodReboot.Models;

namespace APIMoodReboot.Controllers
{
    //[AuthorizeUsers]
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryContent repositoryContent;
        private readonly IRepositoryContentGroups repositoryContentGroups;
        private readonly IRepositoryUsers repositoryUsers;

        public CoursesController(IRepositoryCourses repositoryCourses, IRepositoryContent repositoryContent, IRepositoryContentGroups repositoryContentGroups, IRepositoryUsers repositoryUsers)
        {
            this.repositoryCourses = repositoryCourses;
            this.repositoryContent = repositoryContent;
            this.repositoryContentGroups = repositoryContentGroups;
            this.repositoryUsers = repositoryUsers;
        }

        [HttpGet]
        public async Task<ActionResult> UserCourses()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<CourseListView> courses = await this.repositoryCourses.GetUserCoursesAsync(userId);
            return Ok(courses);
        }

        [HttpDelete("[action]/{courseId}/{userId}")]
        public async Task<ActionResult> DeleteCourseUser(int courseId, int userId)
        {
            Course? course = await this.repositoryCourses.FindCourseAsync(courseId);
            if (course != null)
            {
                await this.repositoryCourses.RemoveCourseUserAsync(courseId, userId);
                if (course.GroupId.HasValue)
                {
                    await this.repositoryUsers.RemoveChatUserAsync(userId, course.GroupId.Value);
                }
            }
            return RedirectToAction("CourseDetails", new { id = courseId });
        }

        [HttpDelete("[action]/{courseId}/{userId}")]
        public async Task<ActionResult> DeleteCourseEditor(int courseId, int userId)
        {
            await this.repositoryCourses.RemoveCourseEditorAsync(courseId, userId);
            return NoContent();
        }

        [HttpPost("[action]/{courseId}/{userId}")]
        public async Task<ActionResult> AddCourseEditor(int courseId, int userId)
        {
            await this.repositoryCourses.AddCourseEditorAsync(courseId, userId);
            return CreatedAtAction(null, null);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateCourse(Course course)
        {
            await this.repositoryCourses.UpdateCourseAsync(course.Id, course.Description, course.Image, course.Name, course.IsVisible);
            return NoContent();
        }

        [HttpGet("[action]/{centerId}")]
        public async Task<ActionResult<List<CourseListView>>> CenterCourses(int centerId)
        {
            return await this.repositoryCourses.GetCenterCoursesAsync(centerId);
        }

        public async Task<ActionResult<List<Course>>> GetAllCourses()
        {
            return await this.repositoryCourses.GetAllCoursesAsync();
        }

        [HttpPost("[action]/{courseId}/{userId}/{password}/{isEditor}")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> CourseEnrollment(int courseId, int userId, string password, bool isEditor = false)
        {
            Course? course = await this.repositoryCourses.FindCourseAsync(courseId);

            if (course == null)
            {
                return NotFound();
            }

            // If the course doesn't have password, enroll the user in the course
            if (course.Password == null)
            {
                await this.repositoryCourses.AddCourseUserAsync(courseId, userId, isEditor);
                return NoContent();
            }

            // If the course has password
            bool added = await this.repositoryCourses.AddCourseUserAsync(courseId, userId, isEditor, password);
            if (added == true)
            {
                return NoContent();
            }

            return Problem("Contraseña del curso incorrecta");
        }

        public async Task<ActionResult> DeleteCourse(int id)
        {
            await this.repositoryCourses.DeleteCourseAsync(id);
            return NoContent();
        }
    }
}
