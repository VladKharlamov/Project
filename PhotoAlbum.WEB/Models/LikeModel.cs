namespace PhotoAlbum.WEB.Models
{
    public class LikeModel
    {
        public string Id { get; set; }

        public string PhotoId { get; set; }

        public string UserId { get; set; }

        public int Count { get; }
    }
}