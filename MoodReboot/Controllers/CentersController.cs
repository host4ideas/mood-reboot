using Microsoft.AspNetCore.Mvc;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Controllers
{
    public class CentersController : Controller
    {
        private readonly IRepositoryCenters repositoryCenters;

        public CentersController(IRepositoryCenters repositoryCenters)
        {
            this.repositoryCenters = repositoryCenters;
        }

        public IActionResult Index()
        {
            List<CenterListView> centers = new()
            {
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY CORTOOOOOOOOOOOOOOOO",
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
                    Telephone = "888888888",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "777777777777777",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "666666666666666",
                    DirectorImage = ""
                },
                new CenterListView()
                {
                    Id= 1,
                    CenterName = "INSTITUTO NOMBRE MUUUUUUUUUY LARGOOOOOOOOOOOOOOOO",
                    Director = "John Doe Doe",
                    Email = "ejemplodeemailmuylargo@ejemplodeemailmuylargo.ejemplodeemailmuylargo",
                    Image = "/images/logos/logo2.jpeg",
                    Telephone = "555555555555555",
                    DirectorImage = ""
                }
            };

            this.repositoryCenters.GetAllCenters();

            return View(centers);
        }

        public IActionResult CenterDetails(int id)
        {
            return View();
        }
    }
}
