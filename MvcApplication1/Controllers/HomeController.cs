using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }


        public ActionResult Links()
        {
            return View();
        }

        public ActionResult ErrorInCS()
        {
            string dummy = null;
            var length = dummy.Length;

            return View();
        }

        public ActionResult MissingView()
        {
            return View();
        }

        public ActionResult AlertWindow()
        {
            return View();
        }

        public ActionResult ConfirmWindow()
        {
            return View();
        }


        public ActionResult PromptWindow()
        {
            return View();
        }

        public ActionResult VariousControls()
        {
            return View();
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
