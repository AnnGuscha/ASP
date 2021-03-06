﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASP.Models;
using JQueryDataTables.Models;

namespace ASP.Controllers
{
    public class ЗаказчикController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Заказчик
        public ActionResult Index()
        {
            return View(db.Заказчик.ToList());
        }

        public ActionResult Home()
        {
            return View(db.Заказчик.ToList());
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.Заказчик.AsEnumerable();
            IEnumerable<Заказчик> filtered;

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = db.Заказчик.AsEnumerable()
                   .Where(c => c.ФИО.ToLower().Contains(param.sSearch.ToLower())
                               || c.Адрес.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Телефон.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Скидка.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<Заказчик, int> orderingFunction = (c => c.КодЗаказчика);
                        filtered = SortHelper<Заказчик, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 1:
                    {
                        Func<Заказчик, string> orderingFunction = (c => c.ФИО);
                        filtered = SortHelper<Заказчик, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 2:
                    {
                        Func<Заказчик, string> orderingFunction = (c => c.Адрес);
                        filtered = SortHelper<Заказчик, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 3:
                    {
                        Func<Заказчик, int?> orderingFunction = (c => c.Телефон);
                        filtered = SortHelper<Заказчик, int?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 4:
                    {
                        Func<Заказчик, int?> orderingFunction = (c => c.Скидка);
                        filtered = SortHelper<Заказчик, int?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB
            var result = from c in displayed select new[] { Convert.ToString(c.КодЗаказчика), c.ФИО, c.Адрес, c.Телефон.ToString(), c.Скидка.ToString() };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: Заказчик/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказчик заказчик = db.Заказчик.Find(id);
            if (заказчик == null)
            {
                return HttpNotFound();
            }
            return View(заказчик);
        }
        [Authorize(Users = "admin")]
        // GET: Заказчик/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Заказчик/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодЗаказчика,ФИО,Адрес,Телефон,Скидка")] Заказчик заказчик)
        {
            if (ModelState.IsValid)
            {
                db.Заказчик.Add(заказчик);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return View(заказчик);
        }
        [Authorize(Users = "admin")]
        // GET: Заказчик/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказчик заказчик = db.Заказчик.Find(id);
            if (заказчик == null)
            {
                return HttpNotFound();
            }
            return View(заказчик);
        }

        // POST: Заказчик/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодЗаказчика,ФИО,Адрес,Телефон,Скидка")] Заказчик заказчик)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказчик).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(заказчик);
        }
        [Authorize(Users = "admin")]
        // GET: Заказчик/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказчик заказчик = db.Заказчик.Find(id);
            if (заказчик == null)
            {
                return HttpNotFound();
            }
            return View(заказчик);
        }

        // POST: Заказчик/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Заказчик заказчик = db.Заказчик.Find(id);
            db.Заказчик.Remove(заказчик);
            db.SaveChanges();
            return RedirectToAction("Home");
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
