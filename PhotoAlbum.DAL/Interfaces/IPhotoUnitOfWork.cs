using System;
using System.Threading.Tasks;
using PhotoAlbum.DAL.Entities;

namespace PhotoAlbum.DAL.Interfaces
{
    public interface IPhotoUnitOfWork : IDisposable
    {
        IUserRepository UserRepository { get; }
        IGenericRepository<UserPhoto> Photos { get; }
        IGenericRepository<Like> Likes { get; }
        IGenericRepository<Comment> Comments { get; }
        IGenericRepository<Like> Categories { get; }
        IGenericRepository<Follow> Followers { get; }
        Task SaveAsync();
        void Save();
    }
}
