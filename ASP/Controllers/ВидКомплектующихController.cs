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
            var allVids = db.ВидКомплектующих.AsEnumerable();
            IEnumerable<ВидКомплектующих> filteredVid;
            //Check whether the companies should be filtered by keyword
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                //Used if particulare columns are filtered 
                var kodFilter = Convert.ToString(Request["sSearch_1"]);
                var nameFilter = Convert.ToString(Request["sSearch_2"]);
                var descFilter = Convert.ToString(Request["sSearch_3"]);

                //Optionally check whether the columns are searchable at all 
                var isKodSearchable = Convert.ToBoolean(Request["bSearchable_1"]);
                var isNameSearchable = Convert.ToBoolean(Request["bSearchable_2"]);
                var isDescrSearchable = Convert.ToBoolean(Request["bSearchable_3"]);

                filteredVid = db.ВидКомплектующих.AsEnumerable()
                   .Where(c => //isKodSearchable && c.КодВида==Convert.ToInt32(param.sSearch)
                               //||
                               isNameSearchable && c.Наименование.ToLower().Contains(param.sSearch.ToLower())
                               ||
                               isDescrSearchable && c.Описание.ToLower().Contains(param.sSearch.ToLower()));
            }
            else
            {
                filteredVid = allVids;
            }

            var isKodSortable = Convert.ToBoolean(Request["bSortable_1"]);
            var isNameSortable = Convert.ToBoolean(Request["bSortable_2"]);
            var isDescrSortable = Convert.ToBoolean(Request["bSortable_3"]);
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            Func<ВидКомплектующих, string> orderingFunction = (c => sortColumnIndex == 1 && isKodSortable ? c.КодВида.ToString() :
                                                           sortColumnIndex == 2 && isNameSortable ? c.Наименование :
                                                           sortColumnIndex == 3 && isDescrSortable ? c.Описание :
                                                           "");

            var sortDirection = Request["sSortDir_0"]; // asc or desc
            if (sortDirection == "asc")
                filteredVid = filteredVid.OrderBy(orderingFunction);
            else
                filteredVid = filteredVid.OrderByDescending(orderingFunction);

            var displayedVids = filteredVid.Skip(param.iDisplayStart).Take(param.iDisplayLength);
            var result = from c in displayedVids select new[] { Convert.ToString(c.КодВида), c.Наименование, c.Описание };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = allVids.Count(),
                iTotalDisplayRecords = filteredVid.Count(),
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
                return RedirectToAction("Index");
            }

            return View(видКомплектующих);
        }

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
                return RedirectToAction("Index");
            }
            return View(видКомплектующих);
        }

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
