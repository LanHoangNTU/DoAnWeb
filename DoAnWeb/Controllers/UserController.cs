using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
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
            ViewBag.MH = db.MATHANGs.Find(id);
            if (commts == null)
            {
                return HttpNotFound();
            }
            int pageSize = 15;
            int pageNumber = (page ?? 1);
            return View(commts.ToPagedList(pageNumber, pageSize));
        }

        public ActionResult AddComment()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AddComment([Bind(Include = "MABL,MAMH,NOIDUNG,NGAYGUI,TENKH")] BINHLUAN bl)
        {
            if (ModelState.IsValid)
            {
                db.BINHLUANs.Add(bl);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(bl);
        }
    }
}