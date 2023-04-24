using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using NugetMoodReboot.Models;
using NugetMoodReboot.Helpers;
using Ganss.Xss;

namespace APIMoodReboot.Controllers
{
    public class ContentController : Controller
    {
        private readonly IRepositoryContent repositoryContent;
        private readonly IRepositoryUsers repositoryUser;
        private readonly HelperFile helperFile;
        private readonly HtmlSanitizer sanitizer;

        public ContentController(IRepositoryContent repositoryContent, IRepositoryUsers repositoryUser, HtmlSanitizer sanitizer, HelperFile helperFile)
        {
            this.repositoryContent = repositoryContent;
            this.repositoryUser = repositoryUser;
            this.sanitizer = sanitizer;
            this.helperFile = helperFile;
        }

        public IActionResult DeleteContent(int contentId, int courseId)
        {
            // If using an asynchronous controller invokes 500 server error
            this.repositoryContent.DeleteContentAsync(contentId).Wait();
            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> AddContent(int userId, int courseId, int groupId, string unsafeHtml, IFormFile hiddenFileInput)
        {
            if (hiddenFileInput != null)
            {
                string mimeType = hiddenFileInput.ContentType;

                string fileName = "content_file_" + await this.repositoryUser.GetMaxFileAsync();
                // Upload file
                // Try with a document
                string? path = await this.helperFile.UploadFileAsync(hiddenFileInput, Folders.ContentFiles, FileTypes.Document, fileName);
                if (path == null)
                {
                    // Try with an image
                    path = await this.helperFile.UploadFileAsync(hiddenFileInput, Folders.ContentFiles, FileTypes.Image, fileName);

                    if (path == null)
                    {
                        ViewData["ERROR"] = "Error al subir archivo. Formatos soportados: .pdf, .xlsx, .jpeg, .jpg, .png, .webp. Tamaño máximo: 10MB.";
                        return RedirectToAction("CourseDetails", "Courses", new { id = courseId });

                    }
                }
                if (path != null)
                {
                    // Insert file in DB
                    int fileId = await this.repositoryUser.InsertFileAsync(path, mimeType, userId);
                    // Update Content
                    await this.repositoryContent.CreateContentFileAsync(contentGroupId: groupId, fileId: fileId);
                }
            }
            else if (unsafeHtml != null)
            {
                string html = unsafeHtml;
                string sanitized = this.sanitizer.Sanitize(html);

                await this.repositoryContent.CreateContentAsync(groupId, sanitized);
            }

            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }

        [HttpPost]
        public async Task<IActionResult> UpdateContent(int userId, int courseId, int contentId, string unsafeHtml, IFormFile hiddenFileInput)
        {
            Content? content = await this.repositoryContent.FindContentAsync(contentId);

            if (content != null)
            {
                if (hiddenFileInput != null)
                {
                    string mimeType = hiddenFileInput.ContentType;
                    string fileName = "content_file_" + contentId;
                    // Upload file                
                    // Try with a document
                    string? path = await this.helperFile.UploadFileAsync(hiddenFileInput, Folders.ContentFiles, FileTypes.Document, fileName);
                    if (path == null)
                    {
                        // Try with an image
                        path = await this.helperFile.UploadFileAsync(hiddenFileInput, Folders.ContentFiles, FileTypes.Image, fileName);

                        if (path == null)
                        {
                            ViewData["ERROR"] = "Error al subir archivo. Formatos soportados: .pdf, .xlsx, .jpeg, .jpg, .png, .webp. Tamaño máximo: 10MB.";
                            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });

                        }
                    }

                    if (path != null)
                    {
                        if (content.FileId == null)
                        {
                            // Update DB
                            int fileId = await this.repositoryUser.InsertFileAsync(path, mimeType, userId);
                            // Update Content
                            await this.repositoryContent.UpdateContentAsync(id: contentId, fileId: fileId);
                        }
                        else
                        {
                            // Update DB
                            int fileId = content.FileId.Value;
                            await this.repositoryUser.UpdateFileAsync(fileId, path, mimeType, userId);
                            // Update Content
                            await this.repositoryContent.UpdateContentAsync(id: contentId, fileId: fileId);
                        }
                    }
                }
                else if (unsafeHtml != null)
                {
                    string sanitized = this.sanitizer.Sanitize(unsafeHtml);

                    await this.repositoryContent.UpdateContentAsync(id: contentId, text: sanitized);
                }
            }

            return RedirectToAction("CourseDetails", "Courses", new { id = courseId });
        }
    }
}
