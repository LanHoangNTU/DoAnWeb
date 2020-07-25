using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using DoAnWeb.Functions;
using DoAnWeb.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace DoAnWeb.Controllers
{
    public class LOAIMATHANGsController : Controller
    {
        private MTPKEntities1 db = new MTPKEntities1();

        // GET: LOAIMATHANGs
        [HttpGet]
        public ActionResult Index(int? page, string name)
        {
            if (Session["admin"] == null)
            {
                return RedirectToAction("LoginForm", "QuanTri");
            }
            var list = db.LOAIMATHANGs.AsQueryable();
            if (!name.IsNullOrWhiteSpace())
            {
                name = IgnoreUnicode.Convert(name);
                var temp = list.ToList().Where(m => IgnoreUnicode.Convert(m.TENLOAI).ToLower().Contains(name.ToLower()));
                list = temp.AsQueryable();
            }
            ViewBag.NAME = name;
            return View(list.OrderBy(m => m.MALOAI).ToPagedList(page ?? 1, 9));
        }

        // GET: LOAIMATHANGs/Details/5
        public ActionResult Details(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAIMATHANG lOAIMATHANG = db.LOAIMATHANGs.Find(id);
            if (lOAIMATHANG == null)
            {
                return HttpNotFound();
            }
            return View(lOAIMATHANG);
        }
        public string GetNewId()
        {
            if (db.LOAIMATHANGs.Count() <= 0)
            {
                return "LOAI000000";
            }
            string id = db.LOAIMATHANGs.Select(m => m.MALOAI).Max();
            int parsed = int.Parse(id.Substring(4, 6)) + 1;
            return "LOAI" + parsed.ToString("000000");
        }
        // GET: LOAIMATHANGs/Create
        public ActionResult Create()
        {
            ViewBag.MA = GetNewId();
            return View();
        }

        // POST: LOAIMATHANGs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "MALOAI,TENLOAI")] LOAIMATHANG lOAIMATHANG)
        {
            if (ModelState.IsValid)
            {
                lOAIMATHANG.MALOAI = GetNewId();
                db.LOAIMATHANGs.Add(lOAIMATHANG);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.MA = GetNewId();
            return View(lOAIMATHANG);
        }

        // GET: LOAIMATHANGs/Edit/5
        public ActionResult Edit(string id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            LOAIMATHANG lOAIMATHANG = db.LOAIMATHANGs.Find(id);
            if (lOAIMATHANG == null)
            {
                return HttpNotFound();
            }
            return View(lOAIMATHANG);
        }

        // POST: LOAIMATHANGs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "MALOAI,TENLOAI")] LOAIMATHANG lOAIMATHANG)
        {
            if (ModelState.IsValid)
            {
                db.Entry(lOAIMATHANG).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Details", new { id = lOAIMATHANG.MALOAI});
            }
            return View(lOAIMATHANG);
        }

        // GET: LOAIMATHANGs/Delete/5
        public ActionResult Delete(string id)
        {
            LOAIMATHANG lOAIMATHANG = db.LOAIMATHANGs.Find(id);
            db.LOAIMATHANGs.Remove(lOAIMATHANG);
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
