using Microsoft.AspNet.Identity.EntityFramework;

namespace PhotoAlbum.DAL.Entities
{
    public class ApplicationUser : IdentityUser
    {
        public  virtual ClientProfile ClientProfile { get; set; }
    }
}
