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
    public class УслугаController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Услуга
        public ActionResult Index()
        {
            return View(db.Услуга.ToList());
        }
        public ActionResult Home()
        {
            return View(db.Услуга.ToList());
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.Услуга.AsEnumerable();
            IEnumerable<Услуга> filtered;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_0"]);
                var nameFilter = Convert.ToString(Request["sSearch_1"]);
                var descFilter = Convert.ToString(Request["sSearch_2"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_0"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isDescrSearchable = Convert.ToBoolean(Request["bSearchable_2"]);

                filtered = db.Услуга.AsEnumerable()
                   .Where(c => //isKodSearchable && c.КодВида==Convert.ToInt32(param.sSearch)
                       //||
                               isNameSearchable && c.Наименование.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDescrSearchable && c.Описание.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            var isKodSortable = Convert.ToBoolean(Request["bSortable_0"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isDescrSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);


            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortColumnIndex == 0 && isKodSortable)
            {
                Func<Услуга, int> orderingFunction = (c => c.КодУслуги);
                filtered = SortHelper<Услуга, int>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 1 && isNameSortable)
            {
                Func<Услуга, string> orderingFunction = (c => c.Наименование);
                filtered = SortHelper<Услуга, string>.Order(sortDirection, filtered, orderingFunction);
            }
            if (sortColumnIndex == 2 && isDescrSortable)
            {
                Func<Услуга, string> orderingFunction = (c => c.Описание);
                filtered = SortHelper<Услуга, string>.Order(sortDirection, filtered, orderingFunction);
            }


            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayed select new[] { Convert.ToString(c.КодУслуги), c.Наименование, c.Описание ,c.Стоимость.ToString()};
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }
        // GET: Услуга/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Услуга услуга = db.Услуга.Find(id);
            if (услуга == null)
            {
                return HttpNotFound();
            }
            return View(услуга);
        }
        [Authorize(Users = "admin")]
        // GET: Услуга/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Услуга/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодУслуги,Наименование,Описание,Стоимость")] Услуга услуга)
        {
            if (ModelState.IsValid)
            {
                db.Услуга.Add(услуга);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return View(услуга);
        }
        [Authorize(Users = "admin")]
        // GET: Услуга/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Услуга услуга = db.Услуга.Find(id);
            if (услуга == null)
            {
                return HttpNotFound();
            }
            return View(услуга);
        }

        // POST: Услуга/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодУслуги,Наименование,Описание,Стоимость")] Услуга услуга)
        {
            if (ModelState.IsValid)
            {
                db.Entry(услуга).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(услуга);
        }

        // GET: Услуга/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Услуга услуга = db.Услуга.Find(id);
            if (услуга == null)
            {
                return HttpNotFound();
            }
            return View(услуга);
        }
        [Authorize(Users = "admin")]
        // POST: Услуга/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Услуга услуга = db.Услуга.Find(id);
            db.Услуга.Remove(услуга);
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
