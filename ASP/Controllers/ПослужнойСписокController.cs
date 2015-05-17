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
    public class ПослужнойСписокController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: ПослужнойСписок
        public ActionResult Index()
        {
            var послужнойСписок = db.ПослужнойСписок.Include(п => п.Должность).Include(п => п.Сотрудник);
            return View(послужнойСписок.ToList());
        }
        public ActionResult Home()
        {
            return View(new PoslugSpisokDTO(db.Сотрудник.ToList(), db.Должность.ToList()));
        }
        public ActionResult AjaxHandler(JQueryDataTableParamModel param)
        {
            var all = db.ПослужнойСписок.AsEnumerable();
            IEnumerable<ПослужнойСписок> filtered;

            //Filter
            int sortId, dolgnId;
            var isNum1 = int.TryParse(Convert.ToString(Request["sSearch_1"]), out sortId);
            var isNum2 = int.TryParse(Convert.ToString(Request["sSearch_2"]), out dolgnId);
            if (isNum1 || isNum2)
            {
                filtered = db.ПослужнойСписок.AsEnumerable()
                .Where(c => c.КодСотрудника == sortId || c.КодДолжности == dolgnId);

            }
            else
            {
                filtered = all;
            }

            //Search
            if (!string.IsNullOrEmpty(param.sSearch))
            {
                filtered = filtered.Where(c => c.Должность.Название.ToLower().Contains(param.sSearch.ToLower())
                               || c.Сотрудник.ФИО.ToLower().Contains(param.sSearch.ToLower())
                               || c.ДатаНазначения.ToString().ToLower().Contains(param.sSearch.ToLower())
                               || c.ДатаОсвобождения.ToString().ToLower().Contains(param.sSearch.ToLower()));
            }

            //Sorting
            var sortColumnIndex = Convert.ToInt32(Request["iSortCol_0"]);
            var sortDirection = Request["sSortDir_0"]; // asc or desc
            switch (sortColumnIndex)
            {
                case 0:
                    {
                        Func<ПослужнойСписок, int> orderingFunction = (c => c.КодСписка);
                        filtered = SortHelper<ПослужнойСписок, int>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 1:
                    {
                        Func<ПослужнойСписок, string> orderingFunction = (c => c.Сотрудник.ФИО);
                        filtered = SortHelper<ПослужнойСписок, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 2:
                    {
                        Func<ПослужнойСписок, string> orderingFunction = (c => c.Должность.Название);
                        filtered = SortHelper<ПослужнойСписок, string>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 3:
                    {
                        Func<ПослужнойСписок, DateTime?> orderingFunction = (c => c.ДатаНазначения);
                        filtered = SortHelper<ПослужнойСписок, DateTime?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
                case 4:
                    {
                        Func<ПослужнойСписок, DateTime?> orderingFunction = (c => c.ДатаОсвобождения);
                        filtered = SortHelper<ПослужнойСписок, DateTime?>.Order(sortDirection, filtered, orderingFunction);
                    }
                    break;
            }

            //Pagination
            var displayed = filtered.Skip(param.iDisplayStart).Take(param.iDisplayLength);

            //Finish selection from DB
            var result = from c in displayed select new[] { Convert.ToString(c.КодСписка), c.Сотрудник.ФИО, c.Должность.Название, ((DateTime)c.ДатаНазначения).ToShortDateString(), ((DateTime)c.ДатаОсвобождения).ToShortDateString() };
            return Json(new
            {
                sEcho = param.sEcho,
                iTotalRecords = all.Count(),
                iTotalDisplayRecords = filtered.Count(),
                aaData = result
            },
                        JsonRequestBehavior.AllowGet);
        }

        // GET: ПослужнойСписок/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ПослужнойСписок послужнойСписок = db.ПослужнойСписок.Find(id);
            if (послужнойСписок == null)
            {
                return HttpNotFound();
            }
            return View(послужнойСписок);
        }

        // GET: ПослужнойСписок/Create
        public ActionResult Create()
        {
            ViewBag.КодДолжности = new SelectList(db.Должность, "КодДолжности", "Название");
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО");
            return View();
        }

        // POST: ПослужнойСписок/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодСписка,КодДолжности,КодСотрудника,ДатаНазначения,ДатаОсвобождения")] ПослужнойСписок послужнойСписок)
        {
            if (ModelState.IsValid)
            {
                db.ПослужнойСписок.Add(послужнойСписок);
                db.SaveChanges();
                return RedirectToAction("Home");
            }

            ViewBag.КодДолжности = new SelectList(db.Должность, "КодДолжности", "Название", послужнойСписок.КодДолжности);
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", послужнойСписок.КодСотрудника);
            return View(послужнойСписок);
        }

        // GET: ПослужнойСписок/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ПослужнойСписок послужнойСписок = db.ПослужнойСписок.Find(id);
            if (послужнойСписок == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодДолжности = new SelectList(db.Должность, "КодДолжности", "Название", послужнойСписок.КодДолжности);
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", послужнойСписок.КодСотрудника);
            return View(послужнойСписок);
        }

        // POST: ПослужнойСписок/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодСписка,КодДолжности,КодСотрудника,ДатаНазначения,ДатаОсвобождения")] ПослужнойСписок послужнойСписок)
        {
            if (ModelState.IsValid)
            {
                db.Entry(послужнойСписок).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Home");
            }
            ViewBag.КодДолжности = new SelectList(db.Должность, "КодДолжности", "Название", послужнойСписок.КодДолжности);
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", послужнойСписок.КодСотрудника);
            return View(послужнойСписок);
        }

        // GET: ПослужнойСписок/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            ПослужнойСписок послужнойСписок = db.ПослужнойСписок.Find(id);
            if (послужнойСписок == null)
            {
                return HttpNotFound();
            }
            return View(послужнойСписок);
        }

        // POST: ПослужнойСписок/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            ПослужнойСписок послужнойСписок = db.ПослужнойСписок.Find(id);
            db.ПослужнойСписок.Remove(послужнойСписок);
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
