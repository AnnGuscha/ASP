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
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_0"]);
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var addresFilter = Convert.ToString(Request["sSearch_2"]);
                var numberFilter = Convert.ToString(Request["sSearch_3"]);
                var saleFilter = Convert.ToString(Request["sSearch_4"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isAddresSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isNumberSearchable = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSaleSearchable = Convert.ToBoolean(Request["bSearchable_4"]);

                filtered = db.Заказчик.AsEnumerable()
                   .Where(c => //isKodSearchable && c.КодВида==Convert.ToInt32(param.sSearch)
                       //||
                               isNameSearchable && c.ФИО.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isAddresSearchable && c.Адрес.ToString().ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isNumberSearchable && c.Телефон.ToString().ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isSaleSearchable && c.Скидка.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            var isKodSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isAddresSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isNumberSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var isSaleSortable = Convert.ToBoolean(Request["bSortable_4"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortColumnIndex == 0 && isKodSortable)
            {
                Func<Заказчик, int> orderingFunction = (c => c.КодЗаказчика);
                filtered = SortHelper<Заказчик, int>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 1 && isNameSortable)
            {
                Func<Заказчик, string> orderingFunction = (c => c.ФИО);
                filtered = SortHelper<Заказчик, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 2 && isAddresSortable)
            {
                Func<Заказчик, string> orderingFunction = (c => c.Адрес);
                filtered = SortHelper<Заказчик, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 3 && isNumberSortable)
            {
                Func<Заказчик, int?> orderingFunction = (c => c.Телефон);
                filtered = SortHelper<Заказчик, int?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 4 && isSaleSortable)
            {
                Func<Заказчик, int?> orderingFunction = (c => c.Скидка);
                filtered = SortHelper<Заказчик, int?>.Order(sortDirection, filtered, orderingFunction);
            }

            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
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
                return RedirectToAction("Index");
            }

            return View(заказчик);
        }

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
                return RedirectToAction("Index");
            }
            return View(заказчик);
        }

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
