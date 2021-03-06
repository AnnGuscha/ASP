﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using ASP.Models;
using JQueryDataTables.Models;

namespace ASP.Controllers
{
    public class КомплектующееController : Controller
    {
        private readonly КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Комплектующее
        public ActionResult Index()
        {
            var комплектующее = db.Комплектующее.Include(к => к.ВидКомплектующих);
            return View(комплектующее.ToList());
        }

        public ActionResult Home()
        {
            return View(new KompDTO(db.ВидКомплектующих.ToList()));
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.Комплектующее.AsEnumerable();
            IEnumerable<Комплектующее> filtered;

            //Filter
            int vidId;
            var isNum = int.TryParse(Convert.ToString(Request["sSearch_2"]), out vidId);
            if (isNum)
            {
                filtered = db.Комплектующее.AsEnumerable()
                    .Where(c => c.КодВида == vidId);
            }
            else
            {
                filtered = all;
            }

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = filtered.Where(c => c.Марка.ToLower().Contains(param.sSearch.ToLower())
                                || c.ВидКомплектующих.Наименование.ToLower().Contains(param.sSearch.ToLower())
                                || c.Цена.ToString().ToLower().Contains(param.sSearch.ToLower())
                                || c.СтранаПроизводитель.ToLower().Contains(param.sSearch.ToLower())
                                || c.ФирмаПроизводитель.ToLower().Contains(param.sSearch.ToLower())
                                || c.ДатаВыпуска.ToString().ToLower().Contains(param.sSearch.ToLower())
                                || c.СрокГарантии.ToString().ToLower().Contains(param.sSearch.ToLower())
                    );
            }

            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<Комплектующее, int> orderingFunction = (c => c.КодКомплектующего);
                        filtered = SortHelper<Комплектующее, int>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 1:
                    {
                        Func<Комплектующее, string> orderingFunction = (c => c.Марка);
                        filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 2:
                    {
                        Func<Комплектующее, double?> orderingFunction = (c => c.Цена);
                        filtered = SortHelper<Комплектующее, double?>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 3:
                    {
                        Func<Комплектующее, string> orderingFunction = (c => c.ВидКомплектующих.Наименование);
                        filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 4:
                    {
                        Func<Комплектующее, string> orderingFunction = (c => c.СтранаПроизводитель);
                        filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 5:
                    {
                        Func<Комплектующее, string> orderingFunction = (c => c.ФирмаПроизводитель);
                        filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 6:
                    {
                        Func<Комплектующее, DateTime?> orderingFunction = (c => c.ДатаВыпуска);
                        filtered = SortHelper<Комплектующее, DateTime?>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 7:
                    {
                        Func<Комплектующее, int?> orderingFunction = (c => c.СрокГарантии);
                        filtered = SortHelper<Комплектующее, int?>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 8:
                    {
                        Func<Комплектующее, string> orderingFunction = (c => c.Описание);
                        filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
                    } break;
                case 9:
                    {
                        Func<Комплектующее, string> orderingFunction = (c => c.Характеристики);
                        filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
                    } break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB ((DateTime) c.ДатаВыпуска).ToShortDateString()
            List<string[]> result = new List<string[]>();
            foreach (var c in displayed)
            {
                string data;
                if (c.ДатаВыпуска == null)
                    data = c.ДатаВыпуска.ToString();
                else
                    data = ((DateTime)c.ДатаВыпуска).ToShortDateString();
                result.Add(new string[] { Convert.ToString(c.КодКомплектующего), c.Марка, c.ВидКомплектующих.Наименование, c.Цена.ToString(),
                        c.СтранаПроизводитель, c.ФирмаПроизводитель, data,
                        c.СрокГарантии.ToString(), c.Описание, c.Характеристики});
            }
            //var result = from c in displayed
            //             select
            //                  new[]
            //        {
            //            Convert.ToString(c.КодКомплектующего), c.Марка, c.ВидКомплектующих.Наименование, c.Цена.ToString(),
            //            c.СтранаПроизводитель, c.ФирмаПроизводитель, c.ДатаВыпуска.ToString(),
            //            c.СрокГарантии.ToString(), c.Описание, c.Характеристики
            //        };

            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                JsonRequestBehavior.AllowGet);
        }

        // GET: Комплектующее/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            return View(комплектующее);
        }
        [Authorize(Users = "admin")]
        // GET: Комплектующее/Create
        public ActionResult Create()
        {
            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование");
            return View();
        }

        // POST: Комплектующее/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(
                Include =
                    "КодКомплектующего,КодВида,Марка,ФирмаПроизводитель,СтранаПроизводитель,ДатаВыпуска,Характеристики,СрокГарантии,Описание,Цена"
                )] Комплектующее комплектующее)
        {
            if (ModelState.IsValid)
            {
                db.Комплектующее.Add(комплектующее);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование", комплектующее.КодВида);
            return View(комплектующее);
        }
        [Authorize(Users = "admin")]
        // GET: Комплектующее/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование", комплектующее.КодВида);
            return View(комплектующее);
        }

        // POST: Комплектующее/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(
            [Bind(
                Include =
                    "КодКомплектующего,КодВида,Марка,ФирмаПроизводитель,СтранаПроизводитель,ДатаВыпуска,Характеристики,СрокГарантии,Описание,Цена"
                )] Комплектующее комплектующее)
        {
            if (ModelState.IsValid)
            {
                db.Entry(комплектующее).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование", комплектующее.КодВида);
            return View(комплектующее);
        }
        [Authorize(Users = "admin")]
        // GET: Комплектующее/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            var комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            return View(комплектующее);
        }

        // POST: Комплектующее/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            var комплектующее = db.Комплектующее.Find(id);
            db.Комплектующее.Remove(комплектующее);
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
