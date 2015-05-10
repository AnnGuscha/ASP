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
    public class ЗаказчикController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Заказчик
        public ActionResult Index()
        {
            return View(db.Заказчик.ToList());
        }

        // GET: Заказчик/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказчик заказчик = db.Заказчик.Find(id);
            if (заказчик == null)
            {
                return HttpNotFound();
            }
            return View(заказчик);
        }

        // GET: Заказчик/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Заказчик/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодЗаказчика,ФИО,Адрес,Телефон,Скидка")] Заказчик заказчик)
        {
            if (ModelState.IsValid)
            {
                db.Заказчик.Add(заказчик);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(заказчик);
        }

        // GET: Заказчик/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказчик заказчик = db.Заказчик.Find(id);
            if (заказчик == null)
            {
                return HttpNotFound();
            }
            return View(заказчик);
        }

        // POST: Заказчик/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодЗаказчика,ФИО,Адрес,Телефон,Скидка")] Заказчик заказчик)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказчик).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(заказчик);
        }

        // GET: Заказчик/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказчик заказчик = db.Заказчик.Find(id);
            if (заказчик == null)
            {
                return HttpNotFound();
            }
            return View(заказчик);
        }

        // POST: Заказчик/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Заказчик заказчик = db.Заказчик.Find(id);
            db.Заказчик.Remove(заказчик);
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
