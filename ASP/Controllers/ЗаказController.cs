using System;
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
    public class ЗаказController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Заказ
        public ActionResult Index()
        {
            var заказ = db.Заказ.Include(з => з.Сотрудник).Include(з => з.Заказчик);
            return View(заказ.ToList());
        }

        public ActionResult Home()
        {
            return View(new ZakazDTO(db.Сотрудник.ToList(),db.Заказчик.ToList()));
        }
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.ВсеЗаказы.AsEnumerable();
            IEnumerable<ВсеЗаказы> filtered;

            //Filter
            var sotrFilter = Convert.ToString(Request["sSearch_1"]);
            var zaklFilter = Convert.ToString(Request["sSearch_2"]);
            if (sotrFilter != "" || zaklFilter != "")
            {
                filtered = db.ВсеЗаказы.AsEnumerable()
                .Where(c => c.Сотрудник == sotrFilter || c.Заказчик == zaklFilter);

            }
            else
            {
                filtered = all;
            }

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = filtered
                   .Where(c => c.Сотрудник.ToLower().Contains(param.sSearch.ToLower())
                               || c.Заказчик.ToLower().Contains(param.sSearch.ToLower())                              
                               || c.Отметки.ToLower().Contains(param.sSearch.ToLower())
                               || c.ДатаЗаказа.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.ДатаИсполнения.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Предоплата.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Стоимость.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Гарантия.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }

            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<ВсеЗаказы, int> orderingFunction = (c => c.КодЗаказа);
                        filtered = SortHelper<ВсеЗаказы, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 1:
                    {
                        Func<ВсеЗаказы, string> orderingFunction = (c => c.Сотрудник);
                        filtered = SortHelper<ВсеЗаказы, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 2:
                    {
                        Func<ВсеЗаказы, string> orderingFunction = (c => c.Заказчик);
                        filtered = SortHelper<ВсеЗаказы, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 3:
                    {
                        Func<ВсеЗаказы, DateTime?> orderingFunction = (c => c.ДатаЗаказа);
                        filtered = SortHelper<ВсеЗаказы, DateTime?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 4:
                    {
                        Func<ВсеЗаказы, DateTime?> orderingFunction = (c => c.ДатаИсполнения);
                        filtered = SortHelper<ВсеЗаказы, DateTime?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 5:
                    {
                        Func<ВсеЗаказы, double?> orderingFunction = (c => c.Предоплата);
                        filtered = SortHelper<ВсеЗаказы, double?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 6:
                    {
                        Func<ВсеЗаказы, double?> orderingFunction = (c => c.Стоимость);
                        filtered = SortHelper<ВсеЗаказы, double?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 7:
                    {
                        Func<ВсеЗаказы, int?> orderingFunction = (c => c.Гарантия);
                        filtered = SortHelper<ВсеЗаказы, int?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 8:
                    {
                        Func<ВсеЗаказы, string> orderingFunction = (c => c.Отметки);
                        filtered = SortHelper<ВсеЗаказы, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB
            var result = from c in displayed select new[] { Convert.ToString(c.КодЗаказа), c.Сотрудник, c.Заказчик, ((DateTime)c.ДатаЗаказа).ToShortDateString(), ((DateTime)c.ДатаИсполнения).ToShortDateString(), c.Предоплата.ToString(), c.Стоимость.ToString(), c.Гарантия.ToString(), c.Отметки };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: Заказ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }

        // GET: Заказ/Create
        public ActionResult Create()
        {
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО");
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО");
            return View();
        }

        // POST: Заказ/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодЗаказа,ДатаЗаказа,ДатаИсполнения,КодЗаказчика,Предоплата,Отметки,Гарантия,КодСотрудника")] Заказ заказ)
        {
            if (ModelState.IsValid)
            {
                db.Заказ.Add(заказ);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", заказ.КодСотрудника);
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО", заказ.КодЗаказчика);
            return View(заказ);
        }

        // GET: Заказ/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", заказ.КодСотрудника);
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО", заказ.КодЗаказчика);
            return View(заказ);
        }

        // POST: Заказ/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодЗаказа,ДатаЗаказа,ДатаИсполнения,КодЗаказчика,Предоплата,Отметки,Гарантия,КодСотрудника")] Заказ заказ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", заказ.КодСотрудника);
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО", заказ.КодЗаказчика);
            return View(заказ);
        }

        // GET: Заказ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }

        // POST: Заказ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Заказ заказ = db.Заказ.Find(id);
            db.Заказ.Remove(заказ);
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
