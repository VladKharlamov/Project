//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using PhotoAlbum.DAL.Entities;

//namespace PhotoAlbum.DAL.EF
//{
//    class PhotoContext : DbContext
//    {
//        public DbSet<ClientPhoto> ClientPhotos { get; set; }
//        public DbSet<Comment> Comments { get; set; }
//        public DbSet<Like> Likes { get; set; }
//        public DbSet<Follow> Followers { get; set; }
//        public DbSet<ClientAvatar> Avatars { get; set; }

//        public PhotoContext(string connectionString)
//            : base(connectionString)
//        {
//        }
//    }
//}
