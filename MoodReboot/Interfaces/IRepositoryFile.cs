using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryFile
    {
        public Task<AppFile> UploadFile(IFormFile file, int userId = -1);
        public Task DeleteFile(int fileId);
    }
}
