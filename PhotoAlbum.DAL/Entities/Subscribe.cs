using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Subscribe
    {
        [Key]
        public string Id { get; set; }

        public ClientProfile User { get; set; }
        public ClientProfile Subcriber { get; set; }

    }
}
