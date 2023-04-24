using Microsoft.AspNetCore.Mvc;
using APIMoodReboot.Helpers;
using APIMoodReboot.Interfaces;
using System.Security.Claims;
using NugetMoodReboot.Helpers;
using NugetMoodReboot.Models;
using APIMoodReboot.Models;

namespace APIMoodReboot.Controllers
{
    public class CentersController : Controller
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

        public async Task<IActionResult> Index()
        {
            List<CenterListView> centers = await this.repositoryCenters.GetAllCentersAsync();
            return View(centers);
        }

        public async Task<IActionResult> CenterDetails(int id)
        {
            Center? center = await this.repositoryCenters.FindCenterAsync(id);
            bool isEditor = false;

            if (HttpContext.User.Identity!.IsAuthenticated == true)
            {
                int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                List<AppUser> users = await this.repositoryCenters.GetCenterEditorsAsync(id);

                foreach (AppUser user in users)
                {
                    if (user.Id == userId)
                    {
                        isEditor = true;
                    }
                }
            }

            if (center == null)
            {
                return RedirectToAction("UserCenters", "Centers");
            }

            List<CourseListView> courses = await this.repositoryCourses.CenterCoursesListViewAsync(id);

            ViewData["IS_EDITOR"] = isEditor;
            ViewData["CENTER"] = center;
            return View(courses);
        }

        #region CENTER USER

        ////[AuthorizeUsers]
        public async Task<IActionResult> UserCenters()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<CenterListView> centers = await this.repositoryCenters.GetUserCentersAsync(userId);
            return View("Index", centers);
        }

        public async Task<IActionResult> RemoveUserCenter(int userId, int centerId)
        {
            await this.repositoryCenters.RemoveUserCenterAsync(userId, centerId);
            return RedirectToAction("DirectorView", new { centerId });
        }

        [HttpPost]
        public async Task<IActionResult> AddCenterEditors(int centerId, List<int> userIds)
        {
            await this.repositoryCenters.AddEditorsCenterAsync(centerId, userIds);
            return RedirectToAction("DirectorView", new { centerId });
        }

        #endregion

        #region EDITOR VIEW

        ////[AuthorizeUsers]
        public async Task<IActionResult> EditorView(int centerId)
        {
            Center? center = await this.repositoryCenters.FindCenterAsync(centerId);
            if (center == null)
            {
                return RedirectToAction("UserCenters", "Centers");
            }

            // Center editor courses where it's editor
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            ViewData["COURSES"] = await this.repositoryCourses.GetEditorCenterCoursesAsync(userId, centerId);

            ViewData["CENTER"] = center;
            return View(new CreateCourseModel());
        }

        //[AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> EditorView(int centerId, string name, bool isVisible, string description, IFormFile image, string password)
        {
            int firstEditorId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            bool result = await this.helperCourse.CreateCourse(centerId, firstEditorId, name, isVisible, image, description, password);

            if (!result)
            {
                ViewData["ERROR"] = "Error al subir el archivo";
            }

            return RedirectToAction("EditorView");
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
