using System.ComponentModel.DataAnnotations;

namespace PhotoAlbum.DAL.Entities
{
    public class Follow
    {
        [Key]
        public string Id { get; set; }

        public ClientProfile User { get; set; }
        public ClientProfile Follower { get; set; }

    }
}
