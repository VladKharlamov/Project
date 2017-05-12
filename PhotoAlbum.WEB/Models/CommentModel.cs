namespace PhotoAlbum.WEB.Models
{
    public class CommentModel
    {
            public string Id { get; set; }

            public string Message { get; set; }

            public string PhotoId { get; set; }

            public string UserId { get; set; }
    }
}