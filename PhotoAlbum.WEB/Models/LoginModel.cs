using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.WEB.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "The field cann`t be empty")]
        [EmailAddress]
        public string Email { get; set; }
        [Required(ErrorMessage = "The field cann`t be empty")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}