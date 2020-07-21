﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using DoAnWeb.Models;
using Microsoft.Ajax.Utilities;
using PagedList;

namespace DoAnWeb.Controllers
{
    public class UserController : Controller
    {
        // GET: CTMH
        private MTPKEntities1 db = new MTPKEntities1();
        [HttpGet]
        public ActionResult IndexLaptop(int? page, string searchString = "", string nsx = "")
        {
            var mATHANGs = db.MATHANGs
                .Where(m => m.MALOAI.Equals("LOAI000001"));
            ViewBag.Search = searchString;
            ViewBag.NSX = nsx;
            if (!searchString.IsNullOrWhiteSpace())
            {
                mATHANGs = mATHANGs.Where(m => m.TENMH.ToLower().Contains(searchString.ToLower()));
            }

            if (!nsx.IsNullOrWhiteSpace())
            {
                mATHANGs = mATHANGs.Where(m => m.MANSX.Equals(nsx));
            }

            mATHANGs = mATHANGs.OrderByDescending(m => m.DANHGIAs.Count);
            int pageSize = 24;
            int pageNumber = (page ?? 1);
            return View(mATHANGs.ToPagedList(pageNumber, pageSize));
        }

        [HttpGet]
        public ActionResult IndexPhuKien(int? page, string id = "LOAI000002", string searchString = "")
        {
            var mATHANGs = db.MATHANGs
                .Where(m => m.MALOAI.Equals(id));

            ViewBag.Search = searchString;
            if (!searchString.IsNullOrWhiteSpace())
            {
                mATHANGs = mATHANGs.Where(m => m.TENMH.ToLower().Contains(searchString.ToLower()));
            }

            ViewBag.MALOAI = id;
            mATHANGs = mATHANGs.OrderByDescending(m => m.DANHGIAs.Count);
            int pageSize = 24;
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
            var mm = db.MATHANGs.Find(id);
            bl.MATHANG = mm;
            int pageNumber = (page ?? 1);
            ViewBag.List = db.MATHANGs.Find(id).BINHLUANs.OrderByDescending(m => m.NGAYGUI)
                .ToPagedList(pageNumber, 15);
            ViewBag.MH = db.MATHANGs.Find(bl.MAMH);
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
            ViewBag.MH = db.MATHANGs.Find(bl.MAMH);
            return View(bl);
        }

        private string GetNewCartId()
        {
            var idMax = db.GIOHANGs.ToList().Select(n => n.MAGH).Max();
            if(idMax == null)
                return "0000000000";

            long newId = long.Parse(idMax) + 1;
            return newId.ToString("0000000000");
        }

        public ActionResult CTGH()
        {
            List<CTGIOHANG> giohang = Session["giohang"] as List<CTGIOHANG>;
            return View(giohang);
        }

        public ActionResult AddToCart(string mh, bool add = true)
        {
            if(Session["giohang"] == null)
            {
                Session["giohang"] = new List<CTGIOHANG>();
            }

            var giohang = Session["giohang"] as List<CTGIOHANG>;

            if(giohang.FirstOrDefault(m => m.MAMH == mh) == null)
            {
                MATHANG sp = db.MATHANGs.Find(mh);

                CTGIOHANG ctx = new CTGIOHANG();
                ctx.MAGH = "0";
                ctx.MAMH = sp.MAMH;
                ctx.MATHANG = sp;
                ctx.SOLUONG = 1;
                ctx.THANHTIEN = (int)(sp.DONGIA - sp.DONGIA * sp.GIAMGIA / 100);

                giohang.Add(ctx);
            }
            else
            {
                CTGIOHANG ct = giohang.FirstOrDefault(m => m.MAMH == mh);
                MATHANG sp = db.MATHANGs.Find(mh);
                if (add)
                {
                    if (ct.SOLUONG < 5)
                        ct.SOLUONG += 1;
                }
                else
                {
                    if(ct.SOLUONG == 1)
                        return RedirectToAction("RemoveFromCart",new { id = mh });
                    ct.SOLUONG -= 1;
                }

                ct.THANHTIEN = ct.SOLUONG * (int)(sp.DONGIA - sp.DONGIA * sp.GIAMGIA / 100);
            }

            return RedirectToAction("CTGH");
        }

        public ActionResult RemoveFromCart(string id)
        {
            List<CTGIOHANG> giohang = Session["giohang"] as List<CTGIOHANG>;
            CTGIOHANG itemXoa = giohang.FirstOrDefault(m => m.MAMH.Equals(id));
            if (itemXoa != null)
            {
                giohang.Remove(itemXoa);
            }
            return RedirectToAction("CTGH");
        }

        public ActionResult Select_1()
        {
            ViewBag.MATP = new SelectList(db.THANHPHOes, "MATP", "TENTP");
            return View();
        }
        [HttpPost]
        public ActionResult Select_2s(FormCollection collection)
        {
            string id = collection["id"];
            ViewBag.DATA = id;
            ViewBag.MAQH = new SelectList(db.QUANHUYENs.Where(x => x.MATP.Equals(id)), "MAQH", "TENQH");;
            return View();
        }
        [HttpPost]
        public ActionResult Select_3s(FormCollection collection)
        {
            if (Session["giohang"] == null)
                return RedirectToAction("IndexLaptop", "User");

            string _id = collection["id"];
            return RedirectToAction("ConfirmBuy", "User", new { id = _id});
        }

        public ActionResult ConfirmBuy(string id)
        {
            ViewBag.MATP = new SelectList(db.PHUONGXAs.Where(x => x.MAQH.Equals(id)), "MAPX", "TENPX");
            ViewBag.MAPTTT = new SelectList(db.PHUONGTHUCTHANHTOANs, "MAPTTT", "TENPTTT");
            return View();
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ConfirmBuy([Bind(Include = "MAGH,MATP,MAPTTT,TRANGTHAI,NGAYXUAT,TONGTIEN,HOTENKH,SDTKH,GIOITINHKH,LOINHAN,DIACHI")] GIOHANG gIOHANG)
        {
            if (Session["giohang"] == null)
            {
                return RedirectToAction("CheckOutResult", "User", new { success = false });
            }

            if (ModelState.IsValid)
            {
                if (gIOHANG.LOINHAN == null || gIOHANG.LOINHAN.Replace(" ", "").Length <= 0)
                    gIOHANG.LOINHAN = "Không có";
                gIOHANG.MAGH = GetNewCartId();
                db.GIOHANGs.Add(gIOHANG);
                db.SaveChanges();
                Session["giohang"] = null;
                return RedirectToAction("CheckOutResult", "User", new { success = true});
            }

            ViewBag.MAPTTT = new SelectList(db.PHUONGTHUCTHANHTOANs, "MAPTTT", "TENPTTT", gIOHANG.MAPTTT);
            ViewBag.MATP = new SelectList(db.PHUONGXAs, "MAPX", "TENPX", gIOHANG.MATP);
            return View(gIOHANG);
        }

        public ActionResult CheckOutResult(bool success)
        {
            ViewBag.SC = success;
            return View();
        }
    }
}