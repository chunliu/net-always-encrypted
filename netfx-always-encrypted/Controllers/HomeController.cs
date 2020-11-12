using netfx_always_encrypted.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace netfx_always_encrypted.Controllers
{
    public class HomeController : Controller
    {
        private TodoContext todoContext = new TodoContext();
        public ActionResult Index()
        {
            var todos = todoContext.TodoItems.ToList();

            return View(todos);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}