using Ganss.Xss;
using Microsoft.AspNetCore.Mvc;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using MoodReboot.Repositories;

namespace MoodReboot.Controllers
{
    public class CoursesController : Controller
    {
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryContent repositoryContent;

        public CoursesController(IRepositoryCourses repositoryCourses, IRepositoryContent repositoryContent)
        {
            this.repositoryCourses = repositoryCourses;
            this.repositoryContent = repositoryContent;
        }

        public IActionResult Index(List<CourseListView> Courses)
        {
            /*
             CourseListView[] Courses =
            {
                new CourseListView()
                {
                    Author = "John Doe Doe Doe",
                    CenterName = "Tajamar",
                    DateModified = DateTime.Now,
                    DatePublished= DateTime.Now,
                    Description = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen.",
                    Id = 0,
                    IsEditor = true,
                    Name = "Mi primer curso de informática"
                },
                new CourseListView()
                {
                    Author = "John Doe Doe Doe",
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    DateModified = null,
                    DatePublished= DateTime.Now,
                    Description = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen.",
                    Id = 0,
                    IsEditor = true,
                    Name = "Mi primer curso de informática"
                },
                new CourseListView()
                {
                    Author = "John Doe Doe Doe",
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    DateModified = null,
                    DatePublished= DateTime.Now,
                    Description = "Lorem Ipsum es simplemente el texto de relleno de las imprentas y archivos de texto. Lorem Ipsum ha sido el texto de relleno estándar de las industrias desde el año 1500, cuando un impresor (N. del T. persona que se dedica a la imprenta) desconocido usó una galería de textos y los mezcló de tal manera que logró hacer un libro de textos especimen.",
                    Id = 0,
                    IsEditor = false,
                    Name = "Mi primer curso de informática"
                }
            };
             */

            return View(Courses);
        }

        public IActionResult UserCourses(int userId)
        {
            List<Course> courses = this.repositoryCourses.GetUserCourses(userId);
            return RedirectToAction("Index", new { Courses = courses });
        }

        public IActionResult CenterCourses(int centerId)
        {
            List<Course> courses = this.repositoryCourses.GetCenterCourses(centerId);
            return RedirectToAction("Index", new { Courses = courses });
        }

        public async Task<IActionResult> CourseDetails(int id)
        {
            List<Content> contents = this.repositoryContent.GetContentsGroup(id);
            Course? course = await this.repositoryCourses.FindCourse(id);

            if (course == null || contents == null)
            {
                return View();
            }

            CourseDetailsModel detais = new()
            {
                Contents = contents,
                Course = course
            };

            return View(detais);
        }

        public IActionResult DeleteCourse(int id)
        {
            this.repositoryCourses.DeleteCourse(id);
            return RedirectToAction("Index");
        }
    }
}
