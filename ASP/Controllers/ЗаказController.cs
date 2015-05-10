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
    public class ЗаказController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Заказ
        public ActionResult Index()
        {
            var заказ = db.Заказ.Include(з => з.Сотрудник).Include(з => з.Заказчик);
            return View(заказ.ToList());
        }

        // GET: Заказ/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }

        // GET: Заказ/Create
        public ActionResult Create()
        {
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО");
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО");
            return View();
        }

        // POST: Заказ/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодЗаказа,ДатаЗаказа,ДатаИсполнения,КодЗаказчика,Предоплата,Отметки,Гарантия,КодСотрудника")] Заказ заказ)
        {
            if (ModelState.IsValid)
            {
                db.Заказ.Add(заказ);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", заказ.КодСотрудника);
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО", заказ.КодЗаказчика);
            return View(заказ);
        }

        // GET: Заказ/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", заказ.КодСотрудника);
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО", заказ.КодЗаказчика);
            return View(заказ);
        }

        // POST: Заказ/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодЗаказа,ДатаЗаказа,ДатаИсполнения,КодЗаказчика,Предоплата,Отметки,Гарантия,КодСотрудника")] Заказ заказ)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказ).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.КодСотрудника = new SelectList(db.Сотрудник, "КодСотрудника", "ФИО", заказ.КодСотрудника);
            ViewBag.КодЗаказчика = new SelectList(db.Заказчик, "КодЗаказчика", "ФИО", заказ.КодЗаказчика);
            return View(заказ);
        }

        // GET: Заказ/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказ заказ = db.Заказ.Find(id);
            if (заказ == null)
            {
                return HttpNotFound();
            }
            return View(заказ);
        }

        // POST: Заказ/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Заказ заказ = db.Заказ.Find(id);
            db.Заказ.Remove(заказ);
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
