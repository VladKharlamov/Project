using System.Collections.Generic;


namespace PhotoAlbum.WEB.Models
{
    public class PhotoPageViewModel
    {
        public IEnumerable<UserPhotoModel> UserPhotos { get; set; }
        public PagingInfo PagingInfo { get; set; }

    }
}