[assembly: WebActivatorEx.PreApplicationStartMethod(typeof(PhotoAlbum.WEB.NinjectWebCommon), "Start")]
[assembly: WebActivatorEx.ApplicationShutdownMethodAttribute(typeof(PhotoAlbum.WEB.NinjectWebCommon), "Stop")]

namespace PhotoAlbum.WEB
{ 
    using System;
    using System.Web;

    using Microsoft.Web.Infrastructure.DynamicModuleHelper;
    using PhotoAlbum.BLL.Interfaces;
    using PhotoAlbum.BLL.Services;
    using PhotoAlbum.BLL.Infrastructure;
    using Ninject;
    using Ninject.Web.Common;
    using Ninject.Modules;
    using System.Configuration;

    public static class NinjectWebCommon 
    {
        private static readonly Bootstrapper bootstrapper = new Bootstrapper();

        /// <summary>
        /// Starts the application
        /// </summary>
        public static void Start() 
        {
            DynamicModuleUtility.RegisterModule(typeof(OnePerRequestHttpModule));
            DynamicModuleUtility.RegisterModule(typeof(NinjectHttpModule));
            bootstrapper.Initialize(CreateKernel);
        }
        
        /// <summary>
        /// Stops the application.
        /// </summary>
        public static void Stop()
        {
            bootstrapper.ShutDown();
        }
        
        /// <summary>
        /// Creates the kernel that will manage your application.
        /// </summary>
        /// <returns>The created kernel.</returns>
        private static IKernel CreateKernel()
        {
            var modules = new INinjectModule[] { new ServiceModule(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString()),  };

            var kernel = new StandardKernel(modules);
            try
            {
                kernel.Bind<Func<IKernel>>().ToMethod(ctx => () => new Bootstrapper().Kernel);
                kernel.Bind<IHttpModule>().To<HttpApplicationInitializationHttpModule>();

                RegisterServices(kernel);
                return kernel;
            }
            catch
            {
                kernel.Dispose();
                throw;
            }
        }

        /// <summary>
        /// Load your modules or register your services here!
        /// </summary>
        /// <param name="kernel">The kernel.</param>
        private static void RegisterServices(IKernel kernel)
        {
            kernel.Bind<IPhotoService>().To<PhotoService>();
            kernel.Bind<IEmailService>().To<EmailService>();
            kernel.Bind<ICommentService>().To<CommentService>();
            kernel.Bind<ILikeService>().To<LikeService>();
            kernel.Bind<ISubcribeService>().To<SubcribeService>();
        }
    }
}
