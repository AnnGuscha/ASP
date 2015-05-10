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
    public class ДолжностьController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Должность
        public ActionResult Index()
        {
            return View(db.Должность.ToList());
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
