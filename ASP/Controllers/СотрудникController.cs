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
    public class СотрудникController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Сотрудник
        public ActionResult Index()
        {
            return View(db.Сотрудник.ToList());
        }

        public ActionResult Home()
        {
            return View(db.Сотрудники.ToList());

        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.Сотрудники.AsEnumerable();
            IEnumerable<Сотрудники> filtered;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_0"]);
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var stagFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDescrSearchable = Convert.ToBoolean(Request["bSearchable_2"]);

                filtered = db.Сотрудники.AsEnumerable()
                   .Where(c => //isKodSearchable && c.КодВида==Convert.ToInt32(param.sSearch)
                       //||
                               isNameSearchable && c.ФИО.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDescrSearchable && c.Стаж.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            var isKodSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isStagSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortColumnIndex == 0 && isKodSortable)
            {
                Func<Сотрудники, int> orderingFunction = (c => c.КодСотрудника);
                filtered = SortHelper<Сотрудники, int>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 1 && isNameSortable)
            {
                Func<Сотрудники, string> orderingFunction = (c => c.ФИО);

                filtered = SortHelper<Сотрудники, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 2 && isStagSortable)
            {
                Func<Сотрудники, int?> orderingFunction =
                    (c => c.Стаж);

                filtered = SortHelper<Сотрудники, int?>.Order(sortDirection, filtered, orderingFunction);
            }


            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayed select new[] { Convert.ToString(c.КодСотрудника), c.ФИО, c.Стаж.ToString() };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: Сотрудник/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        // GET: Сотрудник/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Сотрудник/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодСотрудника,ФИО")] Сотрудник сотрудник)
        {
            if (ModelState.IsValid)
            {
                db.Сотрудник.Add(сотрудник);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return View(сотрудник);
        }

        // GET: Сотрудник/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        // POST: Сотрудник/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодСотрудника,ФИО")] Сотрудник сотрудник)
        {
            if (ModelState.IsValid)
            {
                db.Entry(сотрудник).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(сотрудник);
        }

        // GET: Сотрудник/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            if (сотрудник == null)
            {
                return HttpNotFound();
            }
            return View(сотрудник);
        }

        // POST: Сотрудник/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Сотрудник сотрудник = db.Сотрудник.Find(id);
            db.Сотрудник.Remove(сотрудник);
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
