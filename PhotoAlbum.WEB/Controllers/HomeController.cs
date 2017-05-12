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

        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles="admin")]
        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            
            return View();
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your application contact page.";

            return View();
        }
    }
}