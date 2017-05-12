using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Like
    {
        [Key]
        public string Id { get; set; }

        public ClientPhoto Photo { get; set; }

        public ClientProfile User { get; set; }
    }

}
