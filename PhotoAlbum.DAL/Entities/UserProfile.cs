using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoAlbum.DAL.Entities
{
    public class UserProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public  string Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Follow> Followers { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<UserPhoto> Photos { get; set; }

        public UserProfile()
        {
            Photos = new List<UserPhoto>();
            Likes = new List<Like>();
            Followers = new List<Follow>();
            Comments = new List<Comment>();
        }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
