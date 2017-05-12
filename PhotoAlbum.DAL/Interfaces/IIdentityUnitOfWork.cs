using System;
using System.Threading.Tasks;
using PhotoAlbum.DAL.Identity;

namespace PhotoAlbum.DAL.Interfaces
{
    public interface IIdentityUnitOfWork:IDisposable
    {
        ApplicationUserManager UserManager { get; }
        IClientManager ClientManager { get; }
        ApplicationRoleManager RoleManager { get; }
        Task SaveAsync();
    }
}
