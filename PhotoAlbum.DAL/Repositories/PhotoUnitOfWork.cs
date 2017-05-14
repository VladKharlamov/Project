using System;
using System.Threading.Tasks;
using PhotoAlbum.DAL.EF;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Interfaces;

namespace PhotoAlbum.DAL.Repositories
{
    public class PhotoUnitOfWork : IPhotoUnitOfWork
    {
        private readonly ApplicationContext _db;
        public IClientManager ClientManager { get; }

        public IRepository<ClientPhoto> Photos { get; }

        public IRepository<Like> Likes { get; }

        public IRepository<Comment> Comments { get; }

        public IRepository<Like> Categories { get; }

        public IRepository<Subscribe> Subcribers { get; }


        public PhotoUnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            ClientManager = new ClientManager(_db);
            Photos = new Repository<ClientPhoto>(_db);
            Comments = new Repository<Comment>(_db);
            Likes = new Repository<Like>(_db);
            Categories = new Repository<Like>(_db);
            Subcribers = new Repository<Subscribe>(_db);
        }

        #region Dispose
        private bool disposed = false;
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    _db.Dispose();
                }
                disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public async Task SaveAsync()
        {
            await _db.SaveChangesAsync();
        }
        #endregion



    }
}
