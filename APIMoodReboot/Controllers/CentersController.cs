using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Helpers;
using System.Security.Claims;
using NugetMoodReboot.Models;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using NugetMoodReboot.Interfaces;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CentersController : ControllerBase
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;
        private readonly HelperCourse helperCourse;

        public CentersController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses, HelperCourse helperCourse)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
            this.helperCourse = helperCourse;
        }

        [HttpGet]
        public async Task<ActionResult<List<CenterListView>>> GetCenters()
        {
            return await this.repositoryCenters.GetAllCentersAsync();
        }

        #region CENTER USER

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<CenterListView>>> UserCenters()
        {
            Claim claim = HttpContext.User.Claims.SingleOrDefault(x => x.Type == "UserData");
            string jsonUser = claim.Value;
            AppUser user = JsonConvert.DeserializeObject<AppUser>(jsonUser);

            return await this.repositoryCenters.GetUserCentersAsync(user.Id);
        }

        [Authorize]
        [HttpDelete("{userId}/{centerId}")]
        public async Task<ActionResult> RemoveUserCenter(int userId, int centerId)
        {
            await this.repositoryCenters.RemoveUserCenterAsync(userId, centerId);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{centerId}")]
        public async Task<ActionResult> AddCenterEditors(int centerId, [FromQuery] List<int> userIds)
        {
            await this.repositoryCenters.AddEditorsCenterAsync(centerId, userIds);
            return NoContent();
        }

        #endregion

        #region EDITOR VIEW

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CreateCourse(CreateCourseApiModel newCourse)
        {
            int firstEditorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isVisible = Convert.ToBoolean(newCourse.IsVisible);

            bool result = await this.helperCourse.CreateCourse(newCourse.CenterId, firstEditorId, newCourse.Name, isVisible, newCourse.Image, newCourse.Description, newCourse.Password);

            if (!result)
            {
                return BadRequest("Error al crear el curso");
            }

            return CreatedAtAction(null, null);
        }

        [Authorize]
        [HttpGet("{courseId}")]
        public async Task<ActionResult> DeleteCourse(int courseId)
        {
            await this.repositoryCourses.DeleteCourseAsync(courseId);
            return NoContent();
        }

        [Authorize]
        [HttpGet("{courseId}")]
        public async Task<ActionResult> CourseVisibility(int courseId)
        {
            await this.repositoryCourses.UpdateCourseVisibilityAsync(courseId);
            return NoContent();
        }

        [Authorize]
        [HttpPost("{centerId}")]
        public async Task<ActionResult> VerifyCenter(int centerId)
        {
            Center? center = await this.repositoryCenters.FindCenterAsync(centerId);
            if (center == null)
            {
                return NotFound("El centro no existe");
            }

            return NoContent();
        }

        #endregion

        #region CREATE CENTER

        [Authorize]
        [HttpPost]
        public async Task<ActionResult> CenterRequest(Center center)
        {
            int director = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            await this.repositoryCenters.CreateCenterAsync(center.Email, center.Name, center.Address, center.Telephone, center.Image, director, false);

            return NoContent();
        }

        #endregion
    }
}
