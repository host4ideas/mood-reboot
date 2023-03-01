using Microsoft.AspNetCore.Mvc;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class CoursesController : Controller
    {
        public IActionResult Index()
        {
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

            return View(Courses);
        }

        public IActionResult CourseDetails(int id)
        {
            return View();
        }
    }
}
