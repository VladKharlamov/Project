using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface ILikeService
    {
        bool Like(LikeBLL userPhotoBll);
        LikeBLL GetLikeByUserToPhoto(string userId, string photoId);
        IEnumerable<LikeBLL> GetAllLikesByUser(string userId);
        IEnumerable<LikeBLL> GetLikesByPhoto(string photoId);
    }
}
