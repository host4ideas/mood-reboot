using Microsoft.AspNetCore.WebUtilities;

namespace MoodReboot.Interfaces
{
    public interface IStreamFileUploadService
    {
        Task<bool> UploadFile(MultipartReader reader, MultipartSection section);
    }
}
