using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ninject.Modules;
using PhotoAlbum.BLL.Interfaces;
using PhotoAlbum.DAL.Interfaces;
using PhotoAlbum.DAL.Repositories;

namespace PhotoAlbum.BLL.Infrastructure
{
    public class ServiceModule : NinjectModule
    {
        private string connectionString;
        public ServiceModule(string connection)
        {
            connectionString = connection;
        }
        public override void Load()
        {
            Bind<IIdentityUnitOfWork>().To<IdentityUnitOfWork>().WithConstructorArgument(connectionString);
            Bind<IPhotoUnitOfWork>().To<PhotoUnitOfWork>().WithConstructorArgument(connectionString);

        }
    }
}
