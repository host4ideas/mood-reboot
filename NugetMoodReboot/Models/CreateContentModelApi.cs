namespace NugetMoodReboot.Models
{
    public class CreateContentModelApi
    {
        public int UserId { get; set; }

        public int GroupId { get; set; }

        public string UnsafeHtml { get; set; }

        public AppFile File { get; set; }
    }
}
