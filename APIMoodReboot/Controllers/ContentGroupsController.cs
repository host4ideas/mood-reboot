using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using APIMoodReboot.Repositories;

namespace APIMoodReboot.Controllers
{
    public class ContentGroupsController : Controller
    {
        private readonly IRepositoryContentGroups repo;

        public ContentGroupsController(IRepositoryContentGroups repo)
        {
            this.repo = repo;
        }

        public async Task<IActionResult> DeleteContentGroup(int id, int courseId)
        {
            await this.repo.DeleteContentGroupAsync(id);
            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> CreateContentGroup(string name, int courseId, bool isVisible)
        {
            if (name != null && courseId >= 0)
            {
                await this.repo.CreateContentGroupAsync(name, courseId, isVisible);
            }
            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContentGroup(int id, string name, int courseId, bool isVisible)
        {
            if (name != null && courseId >= 0)
            {
                await this.repo.UpdateContentGroupAsync(id, name, isVisible);
            }
            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }
    }
}
