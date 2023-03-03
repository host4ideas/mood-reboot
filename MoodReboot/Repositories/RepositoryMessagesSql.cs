using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryMessagesSql : IRepositoryMessages
    {
        private MoodRebootContext sqlContext;

        public RepositoryMessagesSql(MoodRebootContext sqlContext)
        {
            this.sqlContext = sqlContext;
        }

        public Task CreateMessage(Message message)
        {
            throw new NotImplementedException();
        }

        public Task DeleteMessage(int id)
        {
            throw new NotImplementedException();
        }

        public List<Message> GetMessagesByGroup()
        {
            throw new NotImplementedException();
        }
    }
}
