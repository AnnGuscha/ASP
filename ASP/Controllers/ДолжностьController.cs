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
    public class ДолжностьController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Должность
        public ActionResult Index()
        {
            return View(db.Должность.ToList());
        }
        public ActionResult Home()
        {
            return View(db.Должность.ToList());
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.Должность.AsEnumerable();
            IEnumerable<Должность> filtered;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_0"]);
                var nameFilter = Convert.ToString(Request["sSearch_1"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);

                filtered = db.Должность.AsEnumerable()
                   .Where(c => isNameSearchable && c.Название.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            var isKodSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortColumnIndex == 0 && isKodSortable)
            {
                Func<Должность, int> orderingFunction = (c => c.КодДолжности);
                filtered = SortHelper<Должность, int>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 1 && isNameSortable)
            {
                Func<Должность, string> orderingFunction = (c => c.Название);

                filtered = SortHelper<Должность, string>.Order(sortDirection, filtered, orderingFunction);
            }

            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayed select new[] { Convert.ToString(c.КодДолжности), c.Название };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
        // GET: Должность/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Должность должность = db.Должность.Find(id);
            if (должность == null)
            {
                return HttpNotFound();
            }
            return View(должность);
        }

        // GET: Должность/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Должность/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодДолжности,Название")] Должность должность)
        {
            if (ModelState.IsValid)
            {
                db.Должность.Add(должность);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(должность);
        }

        // GET: Должность/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Должность должность = db.Должность.Find(id);
            if (должность == null)
            {
                return HttpNotFound();
            }
            return View(должность);
        }

        // POST: Должность/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодДолжности,Название")] Должность должность)
        {
            if (ModelState.IsValid)
            {
                db.Entry(должность).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(должность);
        }

        // GET: Должность/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Должность должность = db.Должность.Find(id);
            if (должность == null)
            {
                return HttpNotFound();
            }
            return View(должность);
        }

        // POST: Должность/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Должность должность = db.Должность.Find(id);
            db.Должность.Remove(должность);
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
