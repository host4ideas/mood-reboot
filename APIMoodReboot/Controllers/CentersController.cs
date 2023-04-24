﻿using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Helpers;
using APIMoodReboot.Interfaces;
using System.Security.Claims;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Models;
using APIMoodReboot.Models;

namespace APIMoodReboot.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CentersController : ControllerBase
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;
        private readonly HelperFile helperFile;
        private readonly HelperMail helperMail;
        private readonly HelperCourse helperCourse;

        public CentersController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses, HelperFile helperFile, HelperMail helperMail, HelperCourse helperCourse)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
            this.helperFile = helperFile;
            this.helperMail = helperMail;
            this.helperCourse = helperCourse;
        }

        [HttpGet]
        public async Task<ActionResult<List<CenterListView>>> GetCenters()
        {
            return await this.repositoryCenters.GetAllCentersAsync();
        }

        #region CENTER USER

        ////[AuthorizeUsers]
        [HttpGet("[action]/{userId}")]
        public async Task<ActionResult<List<CenterListView>>> UserCenters(int userId)
        {
            return await this.repositoryCenters.GetUserCentersAsync(userId);
        }

        ////[AuthorizeUsers]
        [HttpDelete("[action]/{userId}/{centerId}")]
        public async Task<IActionResult> RemoveUserCenter(int userId, int centerId)
        {
            await this.repositoryCenters.RemoveUserCenterAsync(userId, centerId);
            return Ok();
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> AddCenterEditors(int centerId, List<int> userIds)
        {
            await this.repositoryCenters.AddEditorsCenterAsync(centerId, userIds);
            return RedirectToAction("DirectorView", new { centerId });
        }

        #endregion

        #region EDITOR VIEW

        //[AuthorizeUsers]
        [HttpPost("[action]")]
        public async Task<ActionResult> CreateCourse(CreateCourseModel newCourse)
        {
            int firstEditorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool isVisible = Convert.ToBoolean(newCourse.IsVisible);

            bool result = await this.helperCourse.CreateCourse(newCourse.CenterId, firstEditorId, newCourse.Name, isVisible, newCourse.Image, newCourse.Description, newCourse.Password);

            if (!result)
            {
                return BadRequest("Error al crear el curso");
            }

            return Ok();
        }

        public async Task<IActionResult> DeleteCourse(int courseId)
        {
            await this.repositoryCourses.DeleteCourseAsync(courseId);
            return RedirectToAction("EditorView");
        }

        public async Task<IActionResult> CourseVisibility(int courseId)
        {
            await this.repositoryCourses.UpdateCourseVisibilityAsync(courseId);
            return RedirectToAction("EditorView");
        }

        [AcceptVerbs("GET", "POST")]
        public async Task<IActionResult> VerifyCenter(int centerId)
        {
            Center? center = await this.repositoryCenters.FindCenterAsync(centerId);
            if (center == null)
            {
                return Json("El centro no existe");
            }

            return Json(true);
        }

        #endregion

        #region DIRECTOR VIEW

        //[AuthorizeUsers]
        public async Task<IActionResult> DirectorView(int centerId)
        {
            Center? center = await this.repositoryCenters.FindCenterAsync(centerId);
            if (center == null)
            {
                return RedirectToAction("UserCenters", "Centers");
            }

            List<AppUser> users = await this.repositoryCenters.GetCenterEditorsAsync(centerId);
            // Remove the current user from the list
            users.RemoveAll(x => x.Id == int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier)));
            List<CourseListView> courses = await this.repositoryCourses.GetCenterCoursesAsync(centerId);
            ViewData["COURSES"] = courses;
            ViewData["CENTER"] = center;
            return View(users);
        }

        //[AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> DirectorView(int centerId, string centerEmail, string centerName, string centerAddress, string centerTelephone, IFormFile centerImage)
        {
            string fileName = "center_image_" + centerId;

            string? path = await this.helperFile.UploadFileAsync(centerImage, Folders.CenterImages, FileTypes.Image, fileName);

            if (path == null)
            {
                ViewData["ERROR"] = "Error al subir el archivo";
                return RedirectToAction("DirectorView", new { centerId });
            }
            await this.repositoryCenters.UpdateCenterAsync(centerId, centerEmail, centerName, centerAddress, centerTelephone, path);
            return RedirectToAction("DirectorView", new { centerId });
        }

        #endregion

        #region CREATE CENTER

        //[AuthorizeUsers]
        public IActionResult CenterRequest()
        {
            return View();
        }

        //[AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> CenterRequest(string email, string centerEmail, string centerName, string centerAddress, string centerTelephone, IFormFile centerImage)
        {
            int director = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            int maximo = await this.repositoryCenters.GetMaxCenterAsync();

            string fileName = "center_image_" + maximo;

            string? path = await this.helperFile.UploadFileAsync(centerImage, Folders.CenterImages, FileTypes.Image, fileName);

            if (path == null)
            {
                ViewData["ERROR"] = "Error al subir archivo";
                return View();
            }

            await this.repositoryCenters.CreateCenterAsync(centerEmail, centerName, centerAddress, centerTelephone, path, director, false);
            string protocol = HttpContext.Request.IsHttps ? "https" : "http";
            string domainName = HttpContext.Request.Host.Value.ToString();
            string baseUrl = protocol + domainName;
            await this.helperMail.SendMailAsync(email, "Aprobación de centro en curso", "Estamos en proceso de aprobar su solicitud de creación de centro. Por favor, si ha cometido algún error en los datos o quisiera cancelar la operación. Mande un correo a: moodreboot@gmail.com", baseUrl);
            ViewData["MESSAGE"] = "Solicitud enviada";
            return View();
        }

        #endregion
    }
}
