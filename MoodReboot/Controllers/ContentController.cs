using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MvcCoreUtilidades.Helpers;

namespace MoodReboot.Controllers
{
    public class ContentController : Controller
    {
        private readonly IRepositoryContent repositoryContent;
        private readonly IRepositoryFile repositoryFile;
        private readonly HtmlSanitizer sanitizer;
        private readonly HelperFile helperFile;

        public ContentController(IRepositoryContent repositoryContent, IRepositoryFile repositoryFile, HtmlSanitizer sanitizer, HelperFile helperFile)
        {
            this.repositoryContent = repositoryContent;
            this.repositoryFile = repositoryFile;
            this.sanitizer = sanitizer;
            this.helperFile = helperFile;
        }

        public IActionResult DeleteContent(int id, int courseId)
        {
            this.repositoryContent.DeleteContent(id);
            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> AddContent(int userId, int courseId, int groupId, string unsafeHtml, IFormFile file)
        {
            if (file != null)
            {
                string mimeType = file.ContentType;
                string fileName = file.FileName;
                // Upload file                
                await this.helperFile.UploadFileAsync(file, Folders.Temp, fileName);
                // Update DB
                int fileId = await this.repositoryFile.InsertFileAsync(fileName, mimeType, userId);
                // Update Content
                await this.repositoryContent.CreateContentFile(contentGroupId: groupId, fileId: fileId);
            }

            if (unsafeHtml != null)
            {
                string html = unsafeHtml;
                string sanitized = this.sanitizer.Sanitize(html);

                await this.repositoryContent.CreateContent(groupId, sanitized);
            }

            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContent(int userId, int courseId, int contentId, string unsafeHtml, IFormFile file)
        {
            if (file != null)
            {
                string mimeType = file.ContentType;
                string fileName = file.FileName;
                // Upload file                
                await this.helperFile.UploadFileAsync(file, Folders.Temp);
                // Update DB
                int fileId = await this.repositoryFile.InsertFileAsync(fileName, mimeType, userId);
                // Update Content
                await this.repositoryContent.UpdateContent(id: contentId, fileId: fileId);
            }

            if (unsafeHtml != null)
            {
                string sanitized = this.sanitizer.Sanitize(unsafeHtml);

                await this.repositoryContent.UpdateContent(id: contentId, text: sanitized);
            }

            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }
    }
}
