using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;

namespace MoodReboot.Repositories
{
    public class RepositoryCntGroupsSql : IRepositoryContentGroups
    {
        private readonly MoodRebootContext context;

        public RepositoryCntGroupsSql(MoodRebootContext context)
        {
            this.context = context;
        }

        public ContentGroup? FindContentGroup(int id)
        {
            var consulta = from datos in this.context.ContentGroups where datos.ContentGroupId == id select datos;
            return consulta.ToList().FirstOrDefault();
        }

        public List<ContentGroup> GetCourseContentGroups(int courseId)
        {
            var consulta = from datos in this.context.ContentGroups
                           where datos.CourseID == courseId
                           select datos;
            return consulta.ToList();
        }

        public Task CreateContentGroupAsync(string name, int courseId, bool isVisible = false)
        {
            string sql = "SP_CREATE_GROUP_CONTENT @NAME, @COURSE_ID, @IS_VISIBLE, @GROUPCONTENTID OUT";

            int bitIsVisible = 0;

            if (isVisible == true)
            {
                bitIsVisible = 1;
            }

            SqlParameter paramName = new("@NAME", name);
            SqlParameter paramCourseId = new("@COURSE_ID", courseId);
            SqlParameter paramIsVisible = new("@IS_VISIBLE", bitIsVisible);
            SqlParameter paramGroupIdOut = new("@GROUPCONTENTID", 0)
            {
                Direction = System.Data.ParameterDirection.Output,
                SqlDbType = System.Data.SqlDbType.Bit
            };

            return this.context.Database.ExecuteSqlRawAsync(sql, paramName, paramCourseId, paramIsVisible, paramGroupIdOut);
        }

        public async Task UpdateContentGroupAsync(int id, string name, Boolean isVisible)
        {
            ContentGroup? contentGroup = this.FindContentGroup(id);
            if (contentGroup != null)
            {
                contentGroup.Name = name;
                contentGroup.IsVisible = isVisible;
                await this.context.SaveChangesAsync();
            }
        }

        public async Task DeleteContentGroupAsync(int id)
        {
            ContentGroup? group = this.FindContentGroup(id);
            if (group != null)
            {
                this.context.ContentGroups.Remove(group);
                await this.context.SaveChangesAsync();
            }
        }

    }
}
