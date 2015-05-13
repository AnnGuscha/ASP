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
            return View();
        }
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.ВсеЗаказы.AsEnumerable();
            IEnumerable<ВсеЗаказы> filtered;
            //Check whether the companies should be filtered by keyword

            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_0"]);
                var zakazchikFilter = Convert.ToString(Request["sSearch_1"]);
                var sotrudnikFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isSotrudnikSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isZakazchikSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDataZakazaSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isDataIspolneniyaSearchable = Convert.ToBoolean(Request["bSearchable_4"]);
                var isPredoplataSearchable = Convert.ToBoolean(Request["bSearchable_5"]);
                var isStoimostSearchable = Convert.ToBoolean(Request["bSearchable_6"]);
                var isGarantiyaSearchable = Convert.ToBoolean(Request["bSearchable_7"]);
                var isOtmetkiSearchable = Convert.ToBoolean(Request["bSearchable_8"]);


                filtered = db.ВсеЗаказы.AsEnumerable()
                   .Where(c => isSotrudnikSearchable && c.Сотрудник.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isZakazchikSearchable && c.Заказчик.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }
            var isSortableKod = Convert.ToBoolean(Request["bSortable_0"]);
            var isSortableSotrudnik = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortableZakazchik = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortableDataZakaza = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortableDataIspolneniya = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortablePredoplata = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortableStoimost = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortableGarantiya = Convert.ToBoolean(Request["bSortable_7"]);
            var isSortableOtmetki = Convert.ToBoolean(Request["bSortable_8"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortColumnIndex == 0 && isSortableKod)
            {
                Func<ВсеЗаказы, int> orderingFunction = (c => c.КодЗаказа);
                filtered = SortHelper<ВсеЗаказы, int>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 1 && isSortableSotrudnik)
            {
                Func<ВсеЗаказы, string> orderingFunction = (c => c.Сотрудник);
                filtered = SortHelper<ВсеЗаказы, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 2 && isSortableZakazchik)
            {
                Func<ВсеЗаказы, string> orderingFunction = (c => c.Заказчик);
                filtered = SortHelper<ВсеЗаказы, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 3 && isSortableDataZakaza)
            {
                Func<ВсеЗаказы, DateTime?> orderingFunction = (c => c.ДатаЗаказа);
                filtered = SortHelper<ВсеЗаказы, DateTime?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 4 && isSortableDataIspolneniya)
            {
                Func<ВсеЗаказы, DateTime?> orderingFunction = (c => c.ДатаИсполнения);
                filtered = SortHelper<ВсеЗаказы, DateTime?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 5 && isSortablePredoplata)
            {
                Func<ВсеЗаказы, double?> orderingFunction = (c => c.Предоплата);
                filtered = SortHelper<ВсеЗаказы, double?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 6 && isSortableStoimost)
            {
                Func<ВсеЗаказы, double?> orderingFunction = (c => c.Стоимость);
                filtered = SortHelper<ВсеЗаказы, double?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 7 && isSortableGarantiya)
            {
                Func<ВсеЗаказы, int?> orderingFunction = (c => c.Гарантия);
                filtered = SortHelper<ВсеЗаказы, int?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 8 && isSortableOtmetki)
            {
                Func<ВсеЗаказы, string> orderingFunction = (c => c.Отметки);
                filtered = SortHelper<ВсеЗаказы, string>.Order(sortDirection, filtered, orderingFunction);
            }


            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayed select new[] { Convert.ToString(c.КодЗаказа), c.Сотрудник, c.Заказчик, c.ДатаЗаказа.ToString(), c.ДатаИсполнения.ToString(), c.Предоплата.ToString(), c.Стоимость.ToString(), c.Гарантия.ToString(), c.Отметки };
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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
