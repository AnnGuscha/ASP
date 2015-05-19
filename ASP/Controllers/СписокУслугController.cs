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
    public class СписокУслугController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: СписокУслуг
        public ActionResult Index()
        {
            var списокУслуг = db.СписокУслуг.Include(с => с.Заказ).Include(с => с.Услуга);
            return View(списокУслуг.ToList());
        }

        public ActionResult Home()
        {
            return View(new ListUslugDTO(db.Заказ.ToList(), db.Услуга.ToList()));
        }
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.СписокУслуг.AsEnumerable();
            var filtered=all;

            //Filter
            int zakazId;
            var isNum1 = int.TryParse(Convert.ToString(Request["sSearch_1"]), out zakazId);
            if (isNum1)
            {
                filtered = filtered
                .Where(c => c.КодЗаказа == zakazId);

            }

            int uslugaId;
            var isNum2 = int.TryParse(Convert.ToString(Request["sSearch_2"]), out uslugaId);
            if (isNum2)
            {
                filtered = filtered
                .Where(c => c.КодУслуги == uslugaId);

            }

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = filtered.Where(c => c.КодЗаказа.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Услуга.Наименование.ToLower().Contains(param.sSearch.ToLower()));
            }
            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<СписокУслуг, int> orderingFunction = (c => c.КодСписка);
                        filtered = SortHelper<СписокУслуг, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 1:
                    {
                        Func<СписокУслуг, int> orderingFunction = (c => c.КодЗаказа);
                        filtered = SortHelper<СписокУслуг, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 2:
                    {
                        Func<СписокУслуг, string> orderingFunction = (c => c.Услуга.Наименование);
                        filtered = SortHelper<СписокУслуг, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB
            var result = from c in displayed select new[] { Convert.ToString(c.КодСписка), c.КодЗаказа.ToString(), c.Услуга.Наименование };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: СписокУслуг/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            СписокУслуг списокУслуг = db.СписокУслуг.Find(id);
            if (списокУслуг == null)
            {
                return HttpNotFound();
            }
            return View(списокУслуг);
        }

        // GET: СписокУслуг/Create
        public ActionResult Create()
        {
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа");
            ViewBag.КодУслуги = new SelectList(db.Услуга, "КодУслуги", "Наименование");
            return View();
        }

        // POST: СписокУслуг/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодСписка,КодЗаказа,КодУслуги")] СписокУслуг списокУслуг)
        {
            if (ModelState.IsValid)
            {
                db.СписокУслуг.Add(списокУслуг);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа", списокУслуг.КодЗаказа);
            ViewBag.КодУслуги = new SelectList(db.Услуга, "КодУслуги", "Наименование", списокУслуг.КодУслуги);
            return View(списокУслуг);
        }

        // GET: СписокУслуг/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            СписокУслуг списокУслуг = db.СписокУслуг.Find(id);
            if (списокУслуг == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа", списокУслуг.КодЗаказа);
            ViewBag.КодУслуги = new SelectList(db.Услуга, "КодУслуги", "Наименование", списокУслуг.КодУслуги);
            return View(списокУслуг);
        }

        // POST: СписокУслуг/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодСписка,КодЗаказа,КодУслуги")] СписокУслуг списокУслуг)
        {
            if (ModelState.IsValid)
            {
                db.Entry(списокУслуг).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа", списокУслуг.КодЗаказа);
            ViewBag.КодУслуги = new SelectList(db.Услуга, "КодУслуги", "Наименование", списокУслуг.КодУслуги);
            return View(списокУслуг);
        }

        // GET: СписокУслуг/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            СписокУслуг списокУслуг = db.СписокУслуг.Find(id);
            if (списокУслуг == null)
            {
                return HttpNotFound();
            }
            return View(списокУслуг);
        }

        // POST: СписокУслуг/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            СписокУслуг списокУслуг = db.СписокУслуг.Find(id);
            db.СписокУслуг.Remove(списокУслуг);
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
