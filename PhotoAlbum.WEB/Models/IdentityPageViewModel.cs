using System.Collections.Generic;

namespace PhotoAlbum.WEB.Models
{
    public class IdentityPageViewModel
    {
        public IEnumerable<UserModel> Users { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}