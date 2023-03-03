using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class ContentController : Controller
    {
        private readonly IRepositoryContent repositoryContent;
        private readonly IRepositoryFile repositoryFile;
        private readonly HtmlSanitizer sanitizer;

        public ContentController(IRepositoryContent repositoryContent, IRepositoryFile repositoryFile, HtmlSanitizer sanitizer)
        {
            this.repositoryContent = repositoryContent;
            this.repositoryFile = repositoryFile;
            this.sanitizer = sanitizer;
        }

        [HttpPost]
        public async Task<IActionResult> AddContentAsync(int courseId, int groupId, string unsafeHtml)
        {
            string html = unsafeHtml;
            string sanitized = this.sanitizer.Sanitize(html);

            await this.repositoryContent.CreateContent(groupId, sanitized);

            return RedirectToAction("CourseDetails", "CoursesController", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> AddContentFileAsync(int userId, int courseId, int groupId, IFormFile file)
        {
            AppFile createdFile = await this.repositoryFile.UploadFile(file, userId);
            await this.repositoryContent.CreateContentFile(contentGroupId: groupId, fileId: createdFile.Id);

            return RedirectToAction("CourseDetails", "CoursesController", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContentAsync(int id, int groupId, string text)
        {
            string html = text;
            string sanitized = this.sanitizer.Sanitize(html);

            await this.repositoryContent.UpdateContent(id, sanitized);

            return RedirectToAction("CourseDetails", "CoursesController", new { id = groupId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContentFileAsync(Content content, int userId, IFormFile file)
        {
            AppFile createdFile = await this.repositoryFile.UploadFile(file, userId);
            content.File = createdFile;

            await this.repositoryContent.UpdateContent(content.Id, content.Text, content.FileId);

            return RedirectToAction("CourseDetails", "CoursesController", new { id = content.ContentGroupId });
        }
    }
}
