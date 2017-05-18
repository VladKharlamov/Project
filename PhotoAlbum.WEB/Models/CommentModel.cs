using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.WEB.Models
{
    public class CommentModel
    {
        public string Id { get; set; }
        [Required]
        public string Message { get; set; }

        public string PhotoId { get; set; }

        public string UserId { get; set; }
        public DateTime Date { get; set; }
    }
}