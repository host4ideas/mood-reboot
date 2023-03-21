using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MoodReboot.Models
{
    [Table("USER")]
    public class User
    {
        [Key]
        [Column("USER_ID")]
        public int Id { get; set; }
        // Linq
        [Column("EMAIL")]
        // Validations
        [Display(Name = "Email")]
        [Required(ErrorMessage = "Email requerido")]
        [EmailAddress(ErrorMessage = "Email inválido")]
        [Remote(action: "VerifyEmail", controller: "Managed")]
        public string Email { get; set; }
        // Linq
        [Column("PASSWORD")]
        // Validations
        [Display(Name = "Contraseña")]
        [Required(ErrorMessage = "Contraseña requerida")]
        [StringLength(12, ErrorMessage = "{0} debe de estar entre {2} y {1}.", MinimumLength = 6)]
        public byte[] Password { get; set; }
        // Linq
        [Column("USERNAME")]
        // Validations
        [Display(Name = "Nombre de usuario")]
        [Required(ErrorMessage = "Nombre de usuario requerido")]
        [StringLength(20, ErrorMessage = "{0} debe de estar entre {2} y {1}.", MinimumLength = 4)]
        [Remote(action: "VerifyUsername", controller: "Managed")]
        public string UserName { get; set; }
        // Linq
        [Column("FIRST_NAME")]
        // Validations
        [Display(Name = "Nombre")]
        [Required(ErrorMessage = "Nombre requerido")]
        [StringLength(20, ErrorMessage = "{0} debe de estar entre {2} y {1}.", MinimumLength = 2)]
        public string FirstName { get; set; }
        // Linq
        [Column("LAST_NAME")]
        // Validations
        [Display(Name = "Apellidos")]
        [Required(ErrorMessage = "Apellidos requeridos")]
        [StringLength(30, ErrorMessage = "{0} debe de estar entre {2} y {1}.", MinimumLength = 2)]
        public string LastName { get; set; }
        [Column("SIGN_DATE")]
        public DateTime SignedDate { get; set; }
        [Column("IMAGE")]
        public string? Image { get; set; }
        [Column("ROLE")]
        public string Role { get; set; }
        [Column("LAST_SEEN")]
        public DateTime? LastSeen { get; set; }
        [Column("SALT")]
        public string Salt { get; set; }
        [Column("PASS_TEST")]
        public string PassTest { get; set; }
        [Column("APPROVED")]
        public bool Approved { get; set; }
    }
}
