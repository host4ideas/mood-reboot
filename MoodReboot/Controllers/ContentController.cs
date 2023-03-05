using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using static System.Net.Mime.MediaTypeNames;

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
        public async Task<IActionResult> AddContentAsync(int userId, int courseId, int groupId, string unsafeHtml, IFormFile file)
        {
            //if (file != null)
            //{
            //    AppFile createdFile = await this.repositoryFile.UploadFile(file, userId);
            //    await this.repositoryContent.CreateContentFile(contentGroupId: groupId, fileId: createdFile.Id);
            //}

            //if (unsafeHtml != null)
            //{
            //    string html = unsafeHtml;
            //    string sanitized = this.sanitizer.Sanitize(html);

            //    await this.repositoryContent.CreateContent(groupId, sanitized);
            //}

            return RedirectToAction("CourseDetails", "CoursesController", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContentAsync(int userId, int courseId, int contentId, string unsafeHtml, IFormFile file)
        {
            //if (file != null)
            //{
            //    AppFile createdFile = await this.repositoryFile.UploadFile(file, userId);

            //    await this.repositoryContent.UpdateContent(id: contentId, fileId: createdFile.Id);
            //}

            //if (unsafeHtml != null)
            //{
            //    string sanitized = this.sanitizer.Sanitize(unsafeHtml);

            //    await this.repositoryContent.UpdateContent(id: contentId, text: sanitized);
            //}

            return RedirectToAction("CourseDetails", "CoursesController", new { id = courseId });
        }
    }
}
