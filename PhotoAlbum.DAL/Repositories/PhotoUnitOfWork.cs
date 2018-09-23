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
        public IGenericRepository<UserProfile> UserRepository { get; }

        public IGenericRepository<UserPhoto> Photos { get; }

        public IGenericRepository<Like> Likes { get; }

        public IGenericRepository<Comment> Comments { get; }

        public IGenericRepository<Follow> Followers { get; }


        public PhotoUnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            UserRepository = new GenericRepository<UserProfile>(_db);
            Photos = new GenericRepository<UserPhoto>(_db);
            Comments = new GenericRepository<Comment>(_db);
            Likes = new GenericRepository<Like>(_db);
            Followers = new GenericRepository<Follow>(_db);
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
        public void Save()
        {
            _db.SaveChanges();
        }
        #endregion
    }
}
