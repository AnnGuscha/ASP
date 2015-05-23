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
    public class СписокКомплектующихController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: СписокКомплектующих
        [Authorize]
        public ActionResult Index()
        {
            var списокКомплектующих = db.СписокКомплектующих.Include(с => с.Заказ).Include(с => с.Комплектующее);
            return View(списокКомплектующих.ToList());
        }

        [Authorize]
        public ActionResult Home()
        {
            return View(new ListKomplektDTO(db.Заказ.ToList(), db.Комплектующее.ToList()));
        }
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.СписокКомплектующих.AsEnumerable();
            var filtered = all;

            //Filter
            int zakazId;
            var isNum1 = int.TryParse(Convert.ToString(Request["sSearch_1"]), out zakazId);
            if (isNum1)
            {
                filtered = filtered
                .Where(c => c.КодЗаказа == zakazId);

            }

            int komplektId;
            var isNum2 = int.TryParse(Convert.ToString(Request["sSearch_2"]), out komplektId);
            if (isNum2)
            {
                filtered = filtered
                .Where(c => c.КодКомплектующего == komplektId);

            }

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = filtered
                   .Where(c => c.КодЗаказа.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.Комплектующее.Марка.ToLower().Contains(param.sSearch.ToLower()));
            }

            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<СписокКомплектующих, int> orderingFunction = (c => c.КодСписка);
                        filtered = SortHelper<СписокКомплектующих, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 1:
                    {
                        Func<СписокКомплектующих, int> orderingFunction = (c => c.КодЗаказа);
                        filtered = SortHelper<СписокКомплектующих, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 2:
                    {
                        Func<СписокКомплектующих, string> orderingFunction = (c => c.Комплектующее.Марка);
                        filtered = SortHelper<СписокКомплектующих, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB
            var result = from c in displayed select new[] { Convert.ToString(c.КодСписка), c.КодЗаказа.ToString(), c.Комплектующее.Марка };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: СписокКомплектующих/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            СписокКомплектующих списокКомплектующих = db.СписокКомплектующих.Find(id);
            if (списокКомплектующих == null)
            {
                return HttpNotFound();
            }
            return View(списокКомплектующих);
        }
        [Authorize(Users = "admin")]
        // GET: СписокКомплектующих/Create
        public ActionResult Create()
        {
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа");
            ViewBag.КодКомплектующего = new SelectList(db.Комплектующее, "КодКомплектующего", "Марка");
            return View();
        }

        // POST: СписокКомплектующих/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодСписка,КодЗаказа,КодКомплектующего")] СписокКомплектующих списокКомплектующих)
        {
            if (ModelState.IsValid)
            {
                db.СписокКомплектующих.Add(списокКомплектующих);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа", списокКомплектующих.КодЗаказа);
            ViewBag.КодКомплектующего = new SelectList(db.Комплектующее, "КодКомплектующего", "Марка", списокКомплектующих.КодКомплектующего);
            return View(списокКомплектующих);
        }
        [Authorize(Users = "admin")]
        // GET: СписокКомплектующих/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            СписокКомплектующих списокКомплектующих = db.СписокКомплектующих.Find(id);
            if (списокКомплектующих == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа", списокКомплектующих.КодЗаказа);
            ViewBag.КодКомплектующего = new SelectList(db.Комплектующее, "КодКомплектующего", "Марка", списокКомплектующих.КодКомплектующего);
            return View(списокКомплектующих);
        }

        // POST: СписокКомплектующих/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодСписка,КодЗаказа,КодКомплектующего")] СписокКомплектующих списокКомплектующих)
        {
            if (ModelState.IsValid)
            {
                db.Entry(списокКомплектующих).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "КодЗаказа", списокКомплектующих.КодЗаказа);
            ViewBag.КодКомплектующего = new SelectList(db.Комплектующее, "КодКомплектующего", "Марка", списокКомплектующих.КодКомплектующего);
            return View(списокКомплектующих);
        }
        [Authorize(Users = "admin")]
        // GET: СписокКомплектующих/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            СписокКомплектующих списокКомплектующих = db.СписокКомплектующих.Find(id);
            if (списокКомплектующих == null)
            {
                return HttpNotFound();
            }
            return View(списокКомплектующих);
        }

        // POST: СписокКомплектующих/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            СписокКомплектующих списокКомплектующих = db.СписокКомплектующих.Find(id);
            db.СписокКомплектующих.Remove(списокКомплектующих);
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
