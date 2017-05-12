using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class ClientPhoto
    {
        [Key]
        public string Id { get; set; }
        public string PhotoAddress { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool IsAvatar { get; set; } = false;

        public ClientPhoto()
        {
            Comments = new List<Comment>();
            Likes = new List<Like>();
        }

        public ClientProfile User { get; set; }

    }
}