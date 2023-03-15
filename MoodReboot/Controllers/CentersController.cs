﻿using Microsoft.AspNetCore.Mvc;
using MoodReboot.Extensions;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class CentersController : Controller
    {
        private readonly IRepositoryCenters repositoryCenters;
        private readonly IRepositoryCourses repositoryCourses;

        public CentersController(IRepositoryCenters repositoryCenters, IRepositoryCourses repositoryCourses)
        {
            this.repositoryCenters = repositoryCenters;
            this.repositoryCourses = repositoryCourses;
        }

        public async Task<IActionResult> Index()
        {
            List<CenterListView> centers = await this.repositoryCenters.GetAllCenters();
            return View(centers);
        }

        public async Task<IActionResult> CenterDetails(int id)
        {
            Center? center = await this.repositoryCenters.FindCenter(id);

            if (center == null)
            {
                return View();
            }

            List<CourseListView> courses = this.repositoryCourses.CenterCoursesListView(id);

            ViewData["CENTER"] = center;
            return View(courses);
        }

        public async Task<IActionResult> UserCenters()
        {
            UserSession? userSession = HttpContext.Session.GetObject<UserSession>("USER");
            if (userSession != null)
            {
                List<CenterListView> centers = await this.repositoryCenters.GetUserCentersAsync(userSession.UserId);
                return View("Index", centers);
            }
            return View("Index");
        }
    }
}
