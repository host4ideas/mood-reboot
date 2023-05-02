using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using NugetMoodReboot.Models;
using NugetMoodReboot.Helpers;
using Ganss.Xss;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentController : ControllerBase
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

        [HttpDelete("{contentId}")]
        public async Task<ActionResult> DeleteContent(int contentId)
        {
            await this.repositoryContent.DeleteContentAsync(contentId);

            var content = this.repositoryContent.FindContentAsync(contentId);
            if (content == null)
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public async Task<ActionResult> AddContent([FromBody] CreateContentModel createContent)
        {
            if (createContent.File != null)
            {
                IFormFile attachment = createContent.File;
                string mimeType = attachment.ContentType;

                string fileName = "content_file_" + await this.repositoryUser.GetMaxFileAsync();
                // Upload file
                // Try with a document
                string? path = await this.helperFile.UploadFileAsync(attachment, Folders.ContentFiles, FileTypes.Document, fileName);
                if (path == null)
                {
                    // Try with an image
                    path = await this.helperFile.UploadFileAsync(attachment, Folders.ContentFiles, FileTypes.Image, fileName);

                    if (path == null)
                    {
                        return Problem("Error al subir archivo. Formatos soportados: .pdf, .xlsx, .jpeg, .jpg, .png, .webp. Tamaño máximo: 10MB.");
                    }
                }
                if (path != null)
                {
                    // Insert file in DB
                    int fileId = await this.repositoryUser.InsertFileAsync(path, mimeType, createContent.UserId);
                    // Update Content
                    await this.repositoryContent.CreateContentFileAsync(contentGroupId: createContent.GroupId, fileId: fileId);
                }
            }
            else if (createContent.UnsafeHtml != null)
            {
                string html = createContent.UnsafeHtml;
                string sanitized = this.sanitizer.Sanitize(html);

                await this.repositoryContent.CreateContentAsync(createContent.GroupId, sanitized);
            }

            return CreatedAtAction(null, null);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateContent([FromBody] UpdateContentModel updateContent)
        {
            Content? content = await this.repositoryContent.FindContentAsync(updateContent.ContentId);

            if (content == null)
            {
                return NotFound();
            }

            if (updateContent.File != null)
            {
                string mimeType = updateContent.File.ContentType;
                string fileName = "content_file_" + updateContent.ContentId;
                // Upload file                
                // Try with a document
                string? path = await this.helperFile.UploadFileAsync(updateContent.File, Folders.ContentFiles, FileTypes.Document, fileName);
                if (path == null)
                {
                    // Try with an image
                    path = await this.helperFile.UploadFileAsync(updateContent.File, Folders.ContentFiles, FileTypes.Image, fileName);

                    if (path == null)
                    {
                        return Problem("Error al subir archivo. Formatos soportados: .pdf, .xlsx, .jpeg, .jpg, .png, .webp. Tamaño máximo: 10MB.");
                    }
                }

                if (path != null)
                {
                    if (content.FileId == null)
                    {
                        // Update DB
                        int fileId = await this.repositoryUser.InsertFileAsync(path, mimeType, updateContent.UserId);
                        // Update Content
                        await this.repositoryContent.UpdateContentAsync(id: updateContent.ContentId, fileId: fileId);
                    }
                    else
                    {
                        // Update DB
                        int fileId = content.FileId.Value;
                        await this.repositoryUser.UpdateFileAsync(fileId, path, mimeType, updateContent.UserId);
                        // Update Content
                        await this.repositoryContent.UpdateContentAsync(id: updateContent.ContentId, fileId: fileId);
                    }
                }
            }
            else if (updateContent.UnsafeHtml != null)
            {
                string sanitized = this.sanitizer.Sanitize(updateContent.UnsafeHtml);

                await this.repositoryContent.UpdateContentAsync(id: updateContent.ContentId, text: sanitized);
            }

            return NoContent();
        }
    }
}
