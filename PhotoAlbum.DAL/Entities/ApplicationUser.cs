using Microsoft.AspNet.Identity.EntityFramework;

namespace PhotoAlbum.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public  virtual UserProfile UserProfile { get; set; }
    }
}
