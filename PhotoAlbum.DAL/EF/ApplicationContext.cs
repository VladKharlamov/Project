using System.Data.Entity;
using System.Data.Entity.ModelConfiguration;
using Microsoft.AspNet.Identity.EntityFramework;
using PhotoAlbum.DAL.Entities;

namespace PhotoAlbum.DAL.EF
{
    class ApplicationContext:IdentityDbContext<ApplicationUser>
    {
        public ApplicationContext(string conectionString) : base(conectionString) { }
        public DbSet<UserProfile> UserProfiles { get; set; }

        public DbSet<UserPhoto> UserPhotos { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Like> Likes { get; set; }
        public DbSet<Follow> Followers { get; set; }
    }
}
