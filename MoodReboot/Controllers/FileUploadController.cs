using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Net.Http.Headers;
using MoodReboot.Interfaces;

namespace MoodReboot.Controllers
{
    public class FileUploadController : Controller
    {
        readonly IStreamFileUploadService _streamFileUploadService;

        public FileUploadController(IStreamFileUploadService streamFileUploadService)
        {
            _streamFileUploadService = streamFileUploadService;
        }

        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [ActionName("Index")]
        [HttpPost]
        public async Task<IActionResult> SaveFileToPhysicalFolder()
        {
            var boundary = HeaderUtilities.RemoveQuotes(
                MediaTypeHeaderValue.Parse(Request.ContentType).Boundary
            ).Value;

            var reader = new MultipartReader(boundary, Request.Body);

            var section = await reader.ReadNextSectionAsync();

            try
            {
                if (await _streamFileUploadService.UploadFile(reader, section))
                {
                    ViewBag.Message = "File Upload Successful";
                }
                else
                {
                    ViewBag.Message = "File Upload Failed";
                }
            }
            catch (Exception ex)
            {
                //Log ex
                ViewBag.Message = "File Upload Failed: " + ex.Message;
            }
            return RedirectToAction("Home", "Home");
        }
    }
}
