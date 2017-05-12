using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface ICommentService
    {
        void AddComment(CommentBLL commentBll);
        CommentBLL GetComment(string id);
        IEnumerable<CommentBLL> GetCommentsByPhoto(string photoId);
        IEnumerable<CommentBLL> GetCommentsByUser(string userId);
        void EditComment(CommentBLL commentBll);
        void RemoveComment(CommentBLL commentBll);
    }
}
