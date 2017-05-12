using System.Collections.Generic;
using PhotoAlbum.BLL.EnittyBLL;

namespace PhotoAlbum.BLL.Interfaces
{
    public interface IPhotoService
    {
        void AddPhoto(UserPhotoBLL userPhotoBll);
        UserPhotoBLL GetPhoto(string id);
        IEnumerable<UserPhotoBLL> GetPhotos();
        IEnumerable<UserPhotoBLL> GetPhotosByUser(string userId);
        void EditPhoto(UserPhotoBLL userPhotoBll);
        void RemovePhoto(UserPhotoBLL userPhotoBll);
    }
}
