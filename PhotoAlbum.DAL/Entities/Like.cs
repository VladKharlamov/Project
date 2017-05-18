﻿using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Like
    {
        [Key]
        public string Id { get; set; }

        public UserPhoto Photo { get; set; }

        public UserProfile User { get; set; }
    }

}
