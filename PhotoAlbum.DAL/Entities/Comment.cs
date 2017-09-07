using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }

        public string Message { get; set; }

        public UserPhoto Photo { get; set; }

        public UserProfile User { get; set; }
        public DateTime Date { get; set; }
        public string PhotoId { get; set; }

        public string UserId { get; set; }
    }
}
