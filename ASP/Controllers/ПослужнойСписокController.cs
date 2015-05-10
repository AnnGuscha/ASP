using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ASP.Models;

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
                return RedirectToAction("Index");
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
                return RedirectToAction("Index");
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
