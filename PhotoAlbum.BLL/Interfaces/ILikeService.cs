using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface ILikeService
    {
        LikeBLL GetLikeByUserToPhoto(string userId, string photoId);
        IEnumerable<LikeBLL> GetAllLikesByUser(string userId);
        IEnumerable<LikeBLL> GetLikesByPhoto(string photoId);
        int GetCountLikesByPhoto(string photoId);
        void AddLike(LikeBLL likeBll);
        void RemoveLike(LikeBLL likeBll);
    }
}
