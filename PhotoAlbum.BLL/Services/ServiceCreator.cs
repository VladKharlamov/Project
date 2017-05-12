using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.DAL.Repositories;

namespace PhotoAlbum.BLL.Services
{
    public class ServiceCreator : IServiceCreator
    {
        public IUserService CreateUserService(string connection)
        {
            return new UserService(new IdentityUnitOfWork(connection));
        }
    }
}
