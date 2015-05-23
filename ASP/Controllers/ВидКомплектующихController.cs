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
    public class ВидКомплектующихController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        
        // GET: ВидКомплектующих
        public ActionResult Index()
        {
            return View(db.ВидКомплектующих.ToList());
        }
        // GET: ВидКомплектующих
        public ActionResult Home()
        {
            return View(db.ВидКомплектующих.ToList());
        }

        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.ВидКомплектующих.AsEnumerable();
            IEnumerable<ВидКомплектующих> filtered;

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = db.ВидКомплектующих.AsEnumerable()
                   .Where(c => c.Наименование.ToLower().Contains(param.sSearch.ToLower())
                               || c.Описание.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filtered = all;
            }

            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<ВидКомплектующих, int> orderingFunction = (c => c.КодВида);
                        filtered = SortHelper<ВидКомплектующих, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 1:
                    {
                        Func<ВидКомплектующих, string> orderingFunction = (c => c.Наименование);
                        filtered = SortHelper<ВидКомплектующих, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 2:
                    {
                        Func<ВидКомплектующих, string> orderingFunction = (c => c.Описание);
                        filtered = SortHelper<ВидКомплектующих, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB
            var result = from c in displayed select new[] { Convert.ToString(c.КодВида), c.Наименование, c.Описание };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: ВидКомплектующих/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ВидКомплектующих видКомплектующих = db.ВидКомплектующих.Find(id);
            if (видКомплектующих == null)
            {
                return HttpNotFound();
            }
            return View(видКомплектующих);
        }
        [Authorize(Users = "admin")]
        // GET: ВидКомплектующих/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ВидКомплектующих/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодВида,Наименование,Описание")] ВидКомплектующих видКомплектующих)
        {
            if (ModelState.IsValid)
            {
                db.ВидКомплектующих.Add(видКомплектующих);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            return View(видКомплектующих);
        }
        [Authorize(Users = "admin")]
        // GET: ВидКомплектующих/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ВидКомплектующих видКомплектующих = db.ВидКомплектующих.Find(id);
            if (видКомплектующих == null)
            {
                return HttpNotFound();
            }
            return View(видКомплектующих);
        }

        // POST: ВидКомплектующих/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодВида,Наименование,Описание")] ВидКомплектующих видКомплектующих)
        {
            if (ModelState.IsValid)
            {
                db.Entry(видКомплектующих).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            return View(видКомплектующих);
        }
        [Authorize(Users = "admin")]
        // GET: ВидКомплектующих/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ВидКомплектующих видКомплектующих = db.ВидКомплектующих.Find(id);
            if (видКомплектующих == null)
            {
                return HttpNotFound();
            }
            return View(видКомплектующих);
        }

        // POST: ВидКомплектующих/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ВидКомплектующих видКомплектующих = db.ВидКомплектующих.Find(id);
            db.ВидКомплектующих.Remove(видКомплектующих);
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
