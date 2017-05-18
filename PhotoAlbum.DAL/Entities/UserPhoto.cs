using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class UserPhoto
    {
        [Key]
        public string Id { get; set; }
        public string PhotoAddress { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Like> Likes { get; set; }
        public bool IsBlocked { get; set; } = false;
        public bool IsAvatar { get; set; } = false;
        public DateTime Date { get; set; }
        public string Description { get; set; }

        public UserPhoto()
        {
            Comments = new List<Comment>();
            Likes = new List<Like>();
        }

        public UserProfile User { get; set; }

    }
}