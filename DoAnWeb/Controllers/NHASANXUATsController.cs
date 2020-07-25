using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace DoAnWeb.Controllers
{
    public class NHASANXUATsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: NHASANXUATs
        [HttpGet]
        public ActionResult Index(int? page, string name)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            var list = db.NHASANXUATs.AsQueryable();
            if (!name.IsNullOrWhiteSpace())
            {
                list = list.Where(x => x.TENNSX.ToLower().Contains(name.ToLower()));
            }
            ViewBag.NAME = name;
            return View(list.OrderBy(m => m.TENNSX).ToPagedList((page ?? 1), 9));
        }

        // GET: NHASANXUATs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHASANXUAT nHASANXUAT = db.NHASANXUATs.Find(id);
            if (nHASANXUAT == null)
            {
                return HttpNotFound();
            }
            return View(nHASANXUAT);
        }

        public string GetNewId()
        {
            if (db.NHASANXUATs.Count() <= 0)
            {
                return "NSX0000000";
            }
            string id = db.NHASANXUATs.Select(m => m.MANSX).Max();
            int parsed = int.Parse(id.Substring(3, 7)) + 1;
            return "NSX" + parsed.ToString("0000000");
        }
        // GET: NHASANXUATs/Create
        public ActionResult Create()
        {
            ViewBag.NSX = GetNewId();
            return View();
        }

        // POST: NHASANXUATs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MANSX,TENNSX,LOGO")] NHASANXUAT nHASANXUAT)
        {
            var imgNV = Request.Files["Avatar"];
            //Lấy thông tin từ input type=file có tên Avatar
            string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
            //Lưu hình đại diện về Server
            var path = Server.MapPath("/Images/Brand/" + postedFileName);
            imgNV.SaveAs(path);
            if (ModelState.IsValid)
            {
                nHASANXUAT.MANSX = GetNewId();
                nHASANXUAT.LOGO = postedFileName;
                db.NHASANXUATs.Add(nHASANXUAT);
                db.SaveChanges();
                return RedirectToAction("Details", new { id = nHASANXUAT.MANSX});
            }
            ViewBag.NSX = GetNewId();
            return View(nHASANXUAT);
        }

        // GET: NHASANXUATs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            NHASANXUAT nHASANXUAT = db.NHASANXUATs.Find(id);
            if (nHASANXUAT == null)
            {
                return HttpNotFound();
            }
            return View(nHASANXUAT);
        }

        // POST: NHASANXUATs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MANSX,TENNSX,LOGO")] NHASANXUAT nHASANXUAT)
        {
            var imgNV = Request.Files["Avatar"];
            var target = db.NHASANXUATs.Find(nHASANXUAT.MANSX);
            string postedFileName = System.IO.Path.GetFileName(imgNV.FileName);
            //Lu hình đại diện về Server
            var path = Server.MapPath("/Images/Brand/" + postedFileName);
            if (postedFileName != null && !postedFileName.IsNullOrWhiteSpace())
                imgNV.SaveAs(path);
            else
                postedFileName = db.MATHANGs.Find(nHASANXUAT.MANSX).ANH;
            if (ModelState.IsValid)
            {
                nHASANXUAT.LOGO = postedFileName;
                db.Entry(target).CurrentValues.SetValues(nHASANXUAT);
                db.Entry(target).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = target.MANSX});
            }
            return View(nHASANXUAT);
        }

        // GET: NHASANXUATs/Delete/5
        public ActionResult Delete(string id)
        {
            NHASANXUAT nHASANXUAT = db.NHASANXUATs.Find(id);
            db.NHASANXUATs.Remove(nHASANXUAT);
            db.SaveChanges();
            return RedirectToAction("Index");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
