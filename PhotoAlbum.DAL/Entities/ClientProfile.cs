﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PhotoAlbum.DAL.Entities
{
    public class ClientProfile
    {
        [Key]
        [ForeignKey("ApplicationUser")]
        public  string Id { get; set; }

        public string Name { get; set; }

        public DateTime Birthday { get; set; }

        public ICollection<Follow> Subcribes { get; set; }
        public ICollection<Like> Likes { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<ClientPhoto> Photos { get; set; }

        public ClientProfile()
        {
            Photos = new List<ClientPhoto>();
            Likes = new List<Like>();
            Subcribes = new List<Follow>();
            Comments = new List<Comment>();
        }

        public virtual ApplicationUser ApplicationUser { get; set; }
    }
}
