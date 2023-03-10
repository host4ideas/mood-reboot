namespace MvcCoreUtilidades.Helpers
{
    public enum Folders { Images = 0, Files = 1, Temp = 2}

    public class HelperPath
    {
        private IWebHostEnvironment hostEnvironment;

        public HelperPath(IWebHostEnvironment hostEnvironment)
        {
            this.hostEnvironment = hostEnvironment;
        }

        public string MapPath(string fileName, Folders folder)
        {
            string carpeta = "";

            if (folder == Folders.Images)
            {
                carpeta = "images";
            }
            else if (folder == Folders.Files)
            {
                carpeta = "files";
            }
            else if (folder == Folders.Temp)
            {
                carpeta = "temp";
            }

            string rootPath = this.hostEnvironment.WebRootPath;
            string path = Path.Combine(rootPath, "uploads", carpeta, fileName);
            return path;
        }
    }
}
