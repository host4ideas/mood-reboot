﻿using Microsoft.AspNetCore.Mvc;
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

        public async Task<IActionResult> DirectorView(int centerId)
        {
            List<AppUser> users = await this.repositoryCenters.GetCenterEditorsAsync(centerId);
            List<CourseListView> courses = await this.repositoryCourses.GetCenterCourses(centerId);
            ViewData["COURSES"] = courses;
            ViewData["CENTERID"] = centerId;
            return View(users);
        }

        public async Task<IActionResult> CenterDetails(int id)
        {
            Center? center = await this.repositoryCenters.FindCenter(id);
            bool isEditor = false;

            if (HttpContext.User.Identity.IsAuthenticated == true)
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
                return View();
            }

            List<CourseListView> courses = await this.repositoryCourses.CenterCoursesListView(id);

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

        public async Task<IActionResult> RemoveUserCenter(int centerId, int userId)
        {
            await this.repositoryCenters.RemoveUserCenter(centerId, userId);
            return RedirectToAction("DirectorView", new { centerId });
        }

        [HttpPost]
        public async Task<IActionResult> AddCenterEditors(int centerId, List<int> userIds)
        {
            await this.repositoryCenters.AddEditorsCenter(centerId, userIds);
            return RedirectToAction("DirectorView", new { centerId });
        }

        #region CREATE CENTER

        [AuthorizeUsers]
        public IActionResult CenterRequest()
        {
            return View();
        }

        [AuthorizeUsers]
        [HttpPost]
        public async Task<IActionResult> CenterRequest(string email, string centerEmail, string centerName, string centerAddress, string centerTelephone, IFormFile centerImage)
        {
            int director = int.Parse(HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));

            int maximo = await this.repositoryCenters.GetMaxCenter();

            string fileName = "center_image_" + maximo;

            string path = await this.helperFile.UploadFileAsync(centerImage, Folders.CenterImages, fileName);

            await this.repositoryCenters.CreateCenter(centerEmail, centerName, centerAddress, centerTelephone, path, director, false);
            string protocol = HttpContext.Request.IsHttps ? "https" : "http";
            string domainName = HttpContext.Request.Host.Value.ToString();
            string baseUrl = protocol + domainName;
            await this.helperMail.SendMailAsync(email, "Aprobación de centro en curso", "Estamos en proceso de aprobar su solicitud de creación de centro. Por favor, si ha cometido algún error en los datos o quisiera cancelar la operación. Mande un correo a: moodreboot@gmail.com", baseUrl);
            ViewData["MESSAGE"] = "Solicitud enviada";
            ViewData["ERROR"] = "Solicitud enviada";
            return View();
        }

        #endregion
    }
}
