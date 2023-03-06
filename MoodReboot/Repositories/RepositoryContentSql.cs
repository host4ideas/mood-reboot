using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryContentSql : IRepositoryContent
    {
        private readonly MoodRebootContext context;

        public RepositoryContentSql(MoodRebootContext context)
        {
            this.context = context;
        }

        public Task<Content?> FindContent(int id)
        {
            return this.context.Contents.FirstOrDefaultAsync(x => x.Id == id);
        }

        public Task<ContentGroup?> FindContentGroup(int id)
        {
            return this.context.ContentGroups.FirstOrDefaultAsync(x => x.ContentGroupId == id);
        }

        public List<Content> GetContentByGroup(int groupId)
        {
            var consulta = from datos in this.context.Contents
                           where datos.ContentGroupId == groupId
                           select datos;
            return consulta.ToList();
        }

        public List<ContentGroup> GetCourseContentGroups(int courseId)
        {
            var consulta = from datos in this.context.ContentGroups
                           where datos.CourseID == courseId
                           select datos;
            return consulta.ToList();
        }

        public Task CreateContent(int contentGroupId, string text)
        {
            string sql = "SP_CREATE_CONTENT @TEXT, @GROUP_CONTENT_ID";

            SqlParameter paramText = new("@TEXT", text);
            SqlParameter paramGroupId = new("@GROUP_CONTENT_ID", contentGroupId);

            return this.context.Database.ExecuteSqlRawAsync(sql, paramText, paramGroupId);
        }

        public async Task CreateContentFile(int contentGroupId, int fileId)
        {
            string sql = "SP_CREATE_CONTENT @GROUP_CONTENT_ID, @FILE_ID";

            SqlParameter paramGroupId = new("@GROUP_CONTENT_ID", contentGroupId);
            SqlParameter paramFileId = new("@FILE_ID", fileId);

            await this.context.Database.ExecuteSqlRawAsync(sql, paramGroupId, paramFileId);
        }

        public Task CreateContentGroup(string name, int courseId, bool isVisible = false)
        {
            string sql = "SP_CREATE_GROUP_CONTENT @NAME, @COURSE_ID, @IS_VISIBLE, @GROUPCONTENTID OUT";

            SqlParameter paramName = new("@NAME", name);
            SqlParameter paramCourseId = new("@COURSE_ID", courseId);
            SqlParameter paramIsVisible = new("@IS_VISIBLE", isVisible);
            SqlParameter paramGroupIdOut = new("@GROUPCONTENTID", null)
            {
                Direction = System.Data.ParameterDirection.Output
            };

            return this.context.Database.ExecuteSqlRawAsync(sql, paramName, paramCourseId, paramIsVisible, paramGroupIdOut);
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

        public async Task DeleteContentGroup(int id)
        {
            ContentGroup? group = await this.FindContentGroup(id);
            if (group != null)
            {
                this.context.ContentGroups.Remove(group);
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

        public async Task UpdateContentGroup(int id, string name, Boolean isVisible)
        {
            ContentGroup? contentGroup = await this.FindContentGroup(id);
            if (contentGroup != null)
            {
                contentGroup.Name = name;
                contentGroup.IsVisible = isVisible;
                await this.context.SaveChangesAsync();
            }
        }
    }
}
