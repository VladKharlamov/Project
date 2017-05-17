using System.Web.Mvc;

public class ErrorController : Controller
{
    public ActionResult NotFound()
    {
        Response.StatusCode = 404;
        return View();
    }

    public ActionResult Forbidden()
    {
        Response.StatusCode = 403;
        return View();
    }
}