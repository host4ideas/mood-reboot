namespace MoodReboot.Models
{
    public class CenterListView
    {
        public int Id { get; set; }
        public string CenterName { get; set; }
        public string Email { get; set; }
        public string Telephone { get; set; }
        public string Image { get; set; }
        public Author Director { get; set; }
    }
}
