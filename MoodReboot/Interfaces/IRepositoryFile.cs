namespace MoodReboot.Interfaces
{
    public interface IRepositoryFile
    {
        Task DeleteFile(int fileId);
        Task<int> InsertFileAsync(string name, string mimeType);
        Task<int> InsertFileAsync(string name, string mimeType, int userId);
    }
}