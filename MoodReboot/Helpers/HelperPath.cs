namespace MoodReboot.Helpers
{
    public enum Folders { Images = 0, CenterImages = 1, CourseImages = 2, ProfileImages = 3, Files = 4, Temp = 5, Icons = 6, Logos = 7 }

    public class HelperPath
    {
        private readonly IWebHostEnvironment hostEnvironment;

        public HelperPath(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";

            if (folder == Folders.Images)
            {
                carpeta = Path.Combine("uploads", "images");
            }
            if (folder == Folders.CourseImages)
            {
                carpeta = Path.Combine("uploads", "course_images");
            }
            if (folder == Folders.CenterImages)
            {
                carpeta = Path.Combine("uploads", "center_images");
            }
            if (folder == Folders.ProfileImages)
            {
                carpeta = Path.Combine("uploads", "profile_images");
            }
            else if (folder == Folders.Files)
            {
                carpeta = Path.Combine("uploads", "files");
            }
            else if (folder == Folders.Temp)
            {
                carpeta = Path.Combine("uploads", "temp");
            }
            else if (folder == Folders.Icons)
            {
                carpeta = Path.Combine("assets", "icons");
            }
            else if (folder == Folders.Logos)
            {
                carpeta = Path.Combine("assets", "logos");
            }

            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, carpeta, fileName);
            return path;
        }
    }
}
