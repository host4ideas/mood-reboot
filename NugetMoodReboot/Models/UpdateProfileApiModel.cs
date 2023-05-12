using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NugetMoodReboot.Models
{
    public class UpdateProfileApiModel
    {
        public int UserId { get; set; }

        public string Username { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public AppFile? Image { get; set; }
    }
}
