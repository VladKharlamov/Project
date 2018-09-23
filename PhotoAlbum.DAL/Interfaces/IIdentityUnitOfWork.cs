using System;
using System.Threading.Tasks;
using PhotoAlbum.DAL.Entities;
using PhotoAlbum.DAL.Identity;

namespace PhotoAlbum.DAL.Interfaces
{
    public interface IIdentityUnitOfWork:IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IGenericRepository<UserProfile> UserRepository { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
        void Save();
    }
}
