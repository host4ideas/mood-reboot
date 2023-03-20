﻿using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    public class ChatUserModel
    {
        public string UserName { get; set; }
        public int UserID { get; set; }
        public bool IsAdmin { get; set; }
    }
}
