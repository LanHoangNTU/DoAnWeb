using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
using PagedList;

namespace DoAnWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: CTMH
        private MTPKEntities1 db = new MTPKEntities1();
        public ActionResult IndexLaptop(int? page)
        {
            var mATHANGs = db.MATHANGs
                .Where(m => m.MALOAI.Equals("LOAI000001"));

            mATHANGs = mATHANGs.OrderByDescending(m => m.DANHGIAs.Count);
            int pageSize = 3;
            int pageNumber = (page ?? 1);
            return View(mATHANGs.ToPagedList(pageNumber, pageSize));
        }
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            MATHANG mATHANG = db.MATHANGs.Find(id);
            Session["prod"] = mATHANG;
            if (mATHANG == null)
            {
                return HttpNotFound();
            }
            return View(mATHANG);
        }
        public ActionResult Comments(string id, int? page)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var commts = db.MATHANGs.Find(id).BINHLUANs.OrderByDescending(m => m.NGAYGUI);
            if (commts == null)
            {
                return HttpNotFound();
            }
            int pageNumber = (page ?? 1);
            ViewBag.List = db.MATHANGs.Find(id).BINHLUANs.OrderByDescending(m => m.NGAYGUI)
                .ToPagedList(1, 15);
            return RedirectToAction("AddComment", new { id = id, page = page});
        }
        public string NewCommentId()
        {
            var idMax = db.BINHLUANs.ToList().Select(n => n.MABL).Max();
            long newId = long.Parse(idMax) + 1;
            return newId.ToString("000000000000");

        }
        [HttpGet]
        public ActionResult AddComment(string id, int? page)
        {
            BINHLUAN bl = new BINHLUAN();
            bl.MAMH = id;
            int pageNumber = (page ?? 1);
            ViewBag.List = db.MATHANGs.Find(id).BINHLUANs.OrderByDescending(m => m.NGAYGUI)
                .ToPagedList(pageNumber, 15);
            return View(bl);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "MABL,MAMH,NOIDUNG,NGAYGUI,TENKH")] BINHLUAN bl)
        {
            if (ModelState.IsValid)
            {
                bl.MABL = NewCommentId();
                bl.NGAYGUI = DateTime.Now;
                db.BINHLUANs.Add(bl);

                db.SaveChanges();
            }
            ViewBag.List = db.MATHANGs.Find(bl.MAMH).BINHLUANs.OrderByDescending(m => m.NGAYGUI)
                .ToPagedList(1, 15);
            return View(bl);
        }
    }
}