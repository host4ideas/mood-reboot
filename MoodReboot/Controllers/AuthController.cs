﻿using Microsoft.AspNetCore.Mvc;

namespace MoodReboot.Controllers
{
    public class AuthController : Controller
    {
        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            return View();
        }

        public IActionResult Signup()
        {
            return View();
        }
    }
}
