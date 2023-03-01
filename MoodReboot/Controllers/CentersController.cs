using Microsoft.AspNetCore.Mvc;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class CentersController : Controller
    {
        public IActionResult Index()
        {
            CenterListView[] centers = new[]
            {
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "999999999",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO FUZZY",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "999999999",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "999999999",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "999999999",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "999999999",
                    DirectorImage = ""
                }
            };

            return View(centers);
        }

        public IActionResult CenterDetails(int id)
        {
            return View();
        }
    }
}
