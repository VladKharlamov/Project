﻿using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Follow
    {
        [Key]
        public string Id { get; set; }

        public UserProfile User { get; set; }
        public UserProfile Follower { get; set; }

    }
}
