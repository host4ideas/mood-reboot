using APIMoodReboot.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NugetMoodReboot.Models;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize]
    public class FilesController : ControllerBase
    {
        private readonly RepositoryUsersSql repositoryUsers;

        public FilesController(RepositoryUsersSql repositoryUsers)
        {
            this.repositoryUsers = repositoryUsers;
        }

        [HttpPut("{fileId}/{fileName}/{mimeType}")]
        public async Task<ActionResult> UpdateFile(int fileId, string fileName, string mimeType)
        {
            await this.repositoryUsers.UpdateFileAsync(fileId, fileName, mimeType);
            return NoContent();
        }

        [HttpPut("{fileId}/{fileName}/{mimeType}/{userId}")]
        public async Task<ActionResult> UpdateFileUser(int fileId, string fileName, string mimeType, int userId)
        {
            await this.repositoryUsers.UpdateFileAsync(fileId, fileName, mimeType, userId);
            return NoContent();
        }

        [HttpGet("{fileId}")]
        public async Task<ActionResult<AppFile?>> FindFile(int fileId)
        {
            return await this.repositoryUsers.FindFileAsync(fileId);
        }

        [HttpPost("{name}/{mimeType}/{userId}")]
        public async Task<ActionResult<int>> InsertFileUser(string name, string mimeType, int userId)
        {
            return await this.repositoryUsers.InsertFileAsync(name, mimeType, userId);
        }

        [HttpPost("{name}/{mimeType}")]
        public async Task<ActionResult<int>> InsertFile(string name, string mimeType)
        {
            return await this.repositoryUsers.InsertFileAsync(name, mimeType);
        }

        [HttpDelete("{fileId}")]
        public async Task<ActionResult> DeleteFile(int fileId)
        {
            await this.repositoryUsers.DeleteFileAsync(fileId);
            return NoContent();
        }
    }
}
