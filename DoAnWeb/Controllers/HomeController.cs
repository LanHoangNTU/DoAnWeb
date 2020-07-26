using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;

namespace DoAnWeb.Controllers
{
    public class HomeController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();
        public ActionResult Index()
        {
            return View();
        }

        public JsonResult ReturnJSONDataToAJax()
        {
            var json = from s in db.THANHPHOes
                       where s.QUANHUYENs.Where(n => n.PHUONGXAs.Count > 0).Count() > 0
                       select new
                       {
                           Id = s.MATP,
                           Name = s.TENTP
                       };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetQuanHuyensByThanhPhoId(string id)
        {
            var json = from s in db.QUANHUYENs
                       where s.MATP == id &&
                       s.PHUONGXAs.Count > 0
                       select new
                       {
                           Id = s.MAQH,
                           Name = s.TENQH
                       };
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetPXByPHId(string id)
        {
            var json = from s in db.PHUONGXAs
                       where 
                       s.MAQH == id 
                       select new
                       {
                           Id = s.MAPX,
                           Name = s.TENPX
                       };
            return Json(json);
        }

        public JsonResult GetPX(string id)
        {
            var f = db.THANHPHOes.Find(id)
                .QUANHUYENs.ElementAt(0).MAQH;
            var json = from s in db.PHUONGXAs
                       where
                       s.MAQH == f
                       select new
                       {
                           Id = s.MAPX,
                           Name = s.TENPX
                       };
            return Json(json);
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