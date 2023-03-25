using Microsoft.Data.SqlClient;
using MoodReboot.Data;
using MoodReboot.Interfaces;
using MoodReboot.Models;
using System.IO;

namespace MoodReboot.Helpers
{
    public class HelperCourse
    {
        private readonly MoodRebootContext context;
        private readonly IRepositoryCourses repositoryCourses;
        private readonly IRepositoryUsers repositoryUsers;
        private readonly HelperFile helperFile;

        public HelperCourse(MoodRebootContext context, IRepositoryCourses repositoryCourses, IRepositoryUsers repositoryUsers, HelperFile helperFile)
        {
            this.context = context;
            this.repositoryCourses = repositoryCourses;
            this.repositoryUsers = repositoryUsers;
            this.helperFile = helperFile;
        }

        public async Task<bool> CreateCourse(int centerId, int firstEditorId, string name, bool isVisible, IFormFile? image, string? description, string? password)
        {
            // Chat group name max 40 characters
            string chatGroupName = "FORO " + name;
            // Create chat group
            int chatGroupId = await this.repositoryUsers.NewChatGroup(new HashSet<int> { firstEditorId }, firstEditorId, chatGroupName);

            int newCourseId = await this.repositoryCourses.GetMaxCourse();

            string? path = "default_course_image.jpeg";
            // If the user wants an image
            if (image != null)
            {
                string fileName = "course_image_" + newCourseId;
                path = await this.helperFile.UploadFileAsync(image, Folders.CourseImages, FileTypes.Image, fileName);
                if (path == null)
                {
                    return false;
                }
            }

            // Create the course
            await this.context.Courses.AddAsync(new Course()
            {
                CenterId = centerId,
                DateModified = DateTime.Now,
                DatePublished = DateTime.Now,
                Description = description,
                Image = path,
                Password = password,
                GroupId = chatGroupId,
                Id = newCourseId,
                IsVisible = isVisible,
                Name = name,
            });

            await this.context.SaveChangesAsync();
            return true;
        }
    }
}
