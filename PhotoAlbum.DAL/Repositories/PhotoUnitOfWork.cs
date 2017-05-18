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
        public IUserRepository UserRepository { get; }

        public IGenericRepository<UserPhoto> Photos { get; }

        public IGenericRepository<Like> Likes { get; }

        public IGenericRepository<Comment> Comments { get; }

        public IGenericRepository<Like> Categories { get; }

        public IGenericRepository<Follow> Followers { get; }


        public PhotoUnitOfWork(string connectionString)
        {
            _db = new ApplicationContext(connectionString);
            UserRepository = new UserRepository(_db);
            Photos = new GenericGenericRepository<UserPhoto>(_db);
            Comments = new GenericGenericRepository<Comment>(_db);
            Likes = new GenericGenericRepository<Like>(_db);
            Categories = new GenericGenericRepository<Like>(_db);
            Followers = new GenericGenericRepository<Follow>(_db);
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
