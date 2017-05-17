using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Comment
    {
        [Key]
        public string Id { get; set; }

        public string Message { get; set; }

        public ClientPhoto Photo { get; set; }

        public ClientProfile User { get; set; }
        public DateTime Date { get; set; }

    }
}
