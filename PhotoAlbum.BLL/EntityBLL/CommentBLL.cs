using System;

namespace PhotoAlbum.BLL.EnittyBLL
{
    public class CommentBLL
    {
        public string Id { get; set; }

        public string Message { get; set; }

        public string PhotoId { get; set; }

        public string UserId { get; set; }
        public DateTime Date { get; set; }

    }
}
