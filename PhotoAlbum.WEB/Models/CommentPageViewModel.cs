using System.Collections.Generic;


namespace PhotoAlbum.WEB.Models
{
    public class CommentPageViewModel
    {
        public IEnumerable<CommentModel> Comments { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}