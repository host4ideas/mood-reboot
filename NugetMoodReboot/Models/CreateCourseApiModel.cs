﻿namespace NugetMoodReboot.Models
{
    public class CreateCourseApiModel
    {
        public int CenterId { get; set; }
        public string? Description { get; set; }
        public string? Image { get; set; }
        public string Name { get; set; }
        public bool IsVisible { get; set; }
        public string? Password { get; set; }
    }
}