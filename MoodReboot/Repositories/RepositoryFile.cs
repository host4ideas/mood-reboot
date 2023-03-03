using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryFile : IRepositoryFile
    {
        public Task DeleteFile(int fileId)
        {
            throw new NotImplementedException();
        }

        public Task<AppFile> UploadFile(IFormFile file, int userId)
        {
            //string tempFolder = Path.GetTempPath();
            //string fileName = file.FileName;

            //string path = helperPath.MapPath(fileName, Folders.Uploads);

            //using (Stream stream = new FileStream(path, FileMode.Create))
            //{
            //    await file.CopyToAsync(stream);
            //}

            throw new NotImplementedException();
        }
    }
}
