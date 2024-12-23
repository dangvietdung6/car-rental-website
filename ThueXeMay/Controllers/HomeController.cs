
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ThueXeMay.Models;
namespace ThueXeMay.Controllers
{
    public class HomeController : Controller
    {
        RENT_MOTOREntities myObj = new RENT_MOTOREntities();

        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Menu()
        {
            var item = myObj.menus.Where(i => (bool)i.IsActive).OrderBy(j => j.order).ToList();
            return PartialView("Menu", item);
        }
        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Layout_Contact()
        {
            var items = myObj.contacts.Take(1);
            return PartialView("Layout_Contact", items);
        }
        public ActionResult DkDv()
        {
            return View("DK_DV");
        }
        public ActionResult faqs()//tra ve ket qua cau hoi
        {
            return View("FAQs");
        }
    }
}
