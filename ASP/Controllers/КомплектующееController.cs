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
    public class КомплектующееController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Комплектующее
        public ActionResult Index()
        {
            var комплектующее = db.Комплектующее.Include(к => к.ВидКомплектующих);
            return View(комплектующее.ToList());
        }
        public ActionResult Home()
        {
            return View(db.Комплектующее.ToList());
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.Комплектующее.AsEnumerable();
            IEnumerable<Комплектующее> filtered;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_0"]);
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var descFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isSearchable1 = Convert.ToBoolean(Request["bSearchable_1"]);
                var isSearchable2 = Convert.ToBoolean(Request["bSearchable_2"]);
                var isSearchable3 = Convert.ToBoolean(Request["bSearchable_3"]);
                var isSearchable4 = Convert.ToBoolean(Request["bSearchable_4"]);
                var isSearchable5 = Convert.ToBoolean(Request["bSearchable_5"]);
                var isSearchable6 = Convert.ToBoolean(Request["bSearchable_6"]);

                filtered = db.Комплектующее.AsEnumerable()
                   .Where(c => //isKodSearchable && c.КодВида==Convert.ToInt32(param.sSearch)
                       //||
                               isSearchable1 && c.Марка.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isSearchable2 && c.Описание.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            var isKodSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isSortable1 = Convert.ToBoolean(Request["bSortable_1"]);
            var isSortable2 = Convert.ToBoolean(Request["bSortable_2"]);
            var isSortable3 = Convert.ToBoolean(Request["bSortable_3"]);
            var isSortable4 = Convert.ToBoolean(Request["bSortable_4"]);
            var isSortable5 = Convert.ToBoolean(Request["bSortable_5"]);
            var isSortable6 = Convert.ToBoolean(Request["bSortable_6"]);
            var isSortable7 = Convert.ToBoolean(Request["bSortable_7"]);
            var isSortable8 = Convert.ToBoolean(Request["bSortable_8"]);
            var isSortable9 = Convert.ToBoolean(Request["bSortable_9"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortColumnIndex == 0 && isKodSortable)
            {
                Func<Комплектующее, int> orderingFunction = (c => c.КодКомплектующего);
                filtered = SortHelper<Комплектующее, int>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 1 && isSortable1)
            {
                Func<Комплектующее, string> orderingFunction = (c => c.Марка);
                filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 2 && isSortable2)
            {
                Func<Комплектующее, string> orderingFunction = (c => c.ВидКомплектующих.Наименование);
                filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 3 && isSortable3)
            {
                Func<Комплектующее, double?> orderingFunction = (c => c.Цена);
                filtered = SortHelper<Комплектующее, double?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 4 && isSortable4)
            {
                Func<Комплектующее, string> orderingFunction = (c => c.СтранаПроизводитель);
                filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 5 && isSortable5)
            {
                Func<Комплектующее, string> orderingFunction = (c => c.ФирмаПроизводитель);
                filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 6 && isSortable6)
            {
                Func<Комплектующее, DateTime?> orderingFunction = (c => c.ДатаВыпуска);
                filtered = SortHelper<Комплектующее, DateTime?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 7 && isSortable7)
            {
                Func<Комплектующее, int?> orderingFunction = (c => c.СрокГарантии);
                filtered = SortHelper<Комплектующее, int?>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 8 && isSortable8)
            {
                Func<Комплектующее, string> orderingFunction = (c => c.Описание);
                filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 8 && isSortable8)
            {
                Func<Комплектующее, string> orderingFunction = (c => c.Характеристики);
                filtered = SortHelper<Комплектующее, string>.Order(sortDirection, filtered, orderingFunction);
            }


            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayed select new[] { Convert.ToString(c.КодКомплектующего), c.Марка, c.ВидКомплектующих.Наименование, c.Цена.ToString(), c.СтранаПроизводитель, c.ФирмаПроизводитель, c.ДатаВыпуска.ToString(), c.СрокГарантии.ToString(), c.Описание, c.Характеристики };
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
            Комплектующее комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            return View(комплектующее);
        }

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
        public ActionResult Create([Bind(Include = "КодКомплектующего,КодВида,Марка,ФирмаПроизводитель,СтранаПроизводитель,ДатаВыпуска,Характеристики,СрокГарантии,Описание,Цена")] Комплектующее комплектующее)
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

        // GET: Комплектующее/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Комплектующее комплектующее = db.Комплектующее.Find(id);
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
        public ActionResult Edit([Bind(Include = "КодКомплектующего,КодВида,Марка,ФирмаПроизводитель,СтранаПроизводитель,ДатаВыпуска,Характеристики,СрокГарантии,Описание,Цена")] Комплектующее комплектующее)
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

        // GET: Комплектующее/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Комплектующее комплектующее = db.Комплектующее.Find(id);
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
            Комплектующее комплектующее = db.Комплектующее.Find(id);
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
