using Microsoft.AspNetCore.Mvc;
using MoodReboot.Helpers;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MvcCoreSeguridadEmpleados.Filters;
using System.Security.Claims;

namespace MoodReboot.Controllers
{
    public class CentersController : Controller
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;
        private readonly HelperFile helperFile;
        private readonly HelperMail helperMail;

        public CentersController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses, HelperFile helperFile, HelperMail helperMail)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
            this.helperFile = helperFile;
            this.helperMail = helperMail;
        }

        public async Task<IActionResult> Index()
        {
            List<CenterListView> centers = await this.repositoryCenters.GetAllCenters();
            return View(centers);
        }

        public IActionResult EditorView()
        {
            return View();
        }

        public IActionResult DirectorView()
        {
            return View();
        }

        public async Task<IActionResult> CenterDetails(int id)
        {
            Center? center = await this.repositoryCenters.FindCenter(id);
            bool isEditor = false;

            if (HttpContext.User.Identity.IsAuthenticated == true)
            {
                int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
                List<User> users = await this.repositoryCenters.GetCenterEditorsAsync(id);

                foreach (User user in users)
                {
                    if (user.Id == userId)
                    {
                        isEditor = true;
                    }
                }
            }

            if (center == null)
            {
                return View();
            }

            List<CourseListView> courses = this.repositoryCourses.CenterCoursesListView(id);

            ViewData["IS_EDITOR"] = isEditor;
            ViewData["CENTER"] = center;
            return View(courses);
        }

        [AuthorizeUsers]
        public async Task<IActionResult> UserCenters()
        {
            int userId = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            List<CenterListView> centers = await this.repositoryCenters.GetUserCentersAsync(userId);
            return View("Index", centers);
        }

        [AuthorizeUsers]
        public IActionResult CenterRequest()
        {
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> CenterRequest(string contactMail, string centerEmail, string centerName, string centerAddress, string centerTelephone, int director, IFormFile centerImage)
        {
            string fileName = await this.helperFile.UploadFileAsync(centerImage, Folders.CenterImages);
            await this.repositoryCenters.CreateCenter(centerEmail, centerName, centerAddress, centerTelephone, fileName, director, false);
            await this.helperMail.SendMailAsync(contactMail, "Aprobación de centro en curso", "Estamos en proceso de aprobar su solicitud de creación de centro. Por favor, si ha cometido algún error en los datos o quisiera cancelar la operación. Mande un correo a: moodreboot@gmail.com");
            ViewData["MESSAGE"] = "Solicitud enviada";
            return View();
        }
    }
}
