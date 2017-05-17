using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using PhotoAlbum.BLL.Interfaces;

namespace PhotoAlbum.WEB.Controllers
{
    public class HomeController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            ViewBag.Photo = "/Content/MainPhoto.jpg";
            ViewBag.Photo2 = "/Content/MainPhoto2.jpg";

            return View();
        }

        [AllowAnonymous]
        public ActionResult About()
        {
            return View();
        }
        [AllowAnonymous]
        public ActionResult Contact()
        {
            return View();
        }
    }
}