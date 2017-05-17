using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.WEB.Models
{
    public class RegisterModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        [Display(Name = "Confirm password")]
        public string ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime Bithday { get; set; }

        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
    }
}