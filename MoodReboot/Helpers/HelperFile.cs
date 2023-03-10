using MoodReboot.Models;
using MvcCoreUtilidades.Helpers;

namespace MoodReboot.Helpers
{
    public class HelperFile
    {
        private readonly HelperPath helperPath;

        public HelperFile(HelperPath helperPath)
        {
            this.helperPath = helperPath;
        }

        public Task DeleteFile(int fileId)
        {
            throw new NotImplementedException();
        }

        public async Task<string> UploadFileAsync(IFormFile file, Folders folder, string? fileName = "")
        {
            if (fileName != "")
            {
                fileName = file.FileName;
            }

            string path = this.helperPath.MapPath(fileName, folder);

            using (Stream stream = new FileStream(path, FileMode.Create))
            {
                await file.CopyToAsync(stream);
                return path;
            }
        }

        public async Task<List<string>> UploadFilesAsync(List<IFormFile> files, Folders folder)
        {
            List<string> paths = new();

            foreach (IFormFile file in files)
            {
                string fileName = file.FileName;
                string path = this.helperPath.MapPath(fileName, folder);
                paths.Add(path);

                using (Stream stream = new FileStream(path, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }
            }
            return paths;
        }
    }
}
