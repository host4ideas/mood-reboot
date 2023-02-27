using MoodReboot.Models;

namespace MoodReboot.Interfaces
{
    public interface IRepositoryMessages
    {
        public List<Message> GetMessagesByGroup();
        public Task CreateMessage(Message message);
        public Task DeleteMessage(int id);
    }
}
