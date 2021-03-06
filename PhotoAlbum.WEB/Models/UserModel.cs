﻿using System;
using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.WEB.Models
{
    public class UserModel
    {
        public string Id { get; set; }
        [EmailAddress]
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserName { get; set; }
        public string Name { get; set; }
        [DataType(DataType.Date)]
        public DateTime Birthday { get; set; }
        public string Role { get; set; }
        public string Avatar { get; set; }
    }
}