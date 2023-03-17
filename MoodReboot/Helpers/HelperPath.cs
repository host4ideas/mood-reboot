namespace MvcCoreUtilidades.Helpers
{
    public enum Folders { Images = 0, Files = 1, Temp = 2, Icons = 3, Logos = 4 }

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
