using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.WEB.Models
{
    public class UserPhotoModel
    {
        public string Id { get; set; }
        [Required]
        public string PhotoAddress { get; set; }
        [Required]
        public bool IsBlocked { get; set; } = false;
        [Required]
        public bool IsAvatar { get; set; }
        [Required]
        public string UserId { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
