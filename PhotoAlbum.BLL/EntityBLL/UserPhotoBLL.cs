using System;

namespace PhotoAlbum.BLL.EnittyBLL
{
    public class UserPhotoBLL
    {
        public string Id { get; set; }
        public string PhotoAddress { get; set; }
        public bool IsBlocked { get; set; } = false;
        public string UserId { get; set; }
        public bool IsAvatar { get; set; }
        public DateTime Date { get; set; }
        public string Description { get; set; }
    }
}
