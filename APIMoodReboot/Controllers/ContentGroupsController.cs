using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Interfaces;
using NugetMoodReboot.Models;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContentGroupsController : ControllerBase
    {
        private readonly IRepositoryContentGroups repo;

        public ContentGroupsController(IRepositoryContentGroups repo)
        {
            this.repo = repo;
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteContentGroup(int id)
        {
            await this.repo.DeleteContentGroupAsync(id);
            return NoContent();
        }

        [HttpPost("[action]/{name}/{courseId}/{isVisible}")]
        public async Task<ActionResult> CreateContentGroup(string name, int courseId, bool isVisible)
        {
            if (name != null && courseId >= 0)
            {
                await this.repo.CreateContentGroupAsync(name, courseId, isVisible);
            }
            return CreatedAtAction(null, null);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateContentGroup(ContentGroup contentGroup)
        {
            if (contentGroup.Name != null && contentGroup.Name.Any())
            {
                await this.repo.UpdateContentGroupAsync(contentGroup.ContentGroupId, contentGroup.Name, contentGroup.IsVisible);
            }
            return NoContent();
        }
    }
}
