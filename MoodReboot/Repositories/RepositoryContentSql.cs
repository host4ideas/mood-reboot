using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using static System.Net.Mime.MediaTypeNames;

namespace MoodReboot.Repositories
{
    public class RepositoryContentSql : IRepositoryContent
    {
        private readonly MoodRebootContext context;

        public RepositoryContentSql(MoodRebootContext context)
        {
            this.context = context;
        }

        public async Task<int> GetMaxContent()
        {
            return await this.context.Contents.MaxAsync(x => x.Id) + 1;
        }

        public Task<Content?> FindContent(int id)
        {
            return this.context.Contents.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<List<Content>> GetContentByGroup(int groupId)
        {
            return this.context.Contents.Where(x => x.ContentGroupId == groupId).ToListAsync();
        }

        public async Task CreateContent(int contentGroupId, string text)
        {
            Content content = new()
            {
                Id = await this.GetMaxContent(),
                Text = text,
                ContentGroupId = contentGroupId,
            };

            await this.context.Contents.AddAsync(content);
            await this.context.SaveChangesAsync();
        }

        public async Task CreateContentFile(int contentGroupId, int fileId)
        {
            Content content = new()
            {
                Id = await this.GetMaxContent(),
                ContentGroupId = contentGroupId,
                FileId = fileId
            };

            await this.context.Contents.AddAsync(content);
            await this.context.SaveChangesAsync();
        }

        public async Task DeleteContent(int id)
        {
            Content? content = await this.FindContent(id);
            if (content != null)
            {
                this.context.Contents.Remove(content);
                await this.context.SaveChangesAsync();
            }
        }

        public async Task UpdateContent(int id, string? text = null, int? fileId = null)
        {
            Content? oldContent = await this.FindContent(id);
            if (oldContent != null)
            {
                oldContent.Text = text;
                oldContent.FileId = fileId;
                await this.context.SaveChangesAsync();
            }
        }
    }
}
