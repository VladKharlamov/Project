using System;
using System.Threading.Tasks;
using PhotoAlbum.DAL.Entities;

namespace PhotoAlbum.DAL.Interfaces
{
    public interface IPhotoUnitOfWork : IDisposable
    {
        IClientManager ClientManager { get; }
        IRepository<ClientPhoto> Photos { get; }
        IRepository<Like> Likes { get; }
        IRepository<Comment> Comments { get; }
        IRepository<Like> Categories { get; }
        IRepository<Subscribe> Subcribers { get; }
        Task SaveAsync();
        void Save();
    }
}
