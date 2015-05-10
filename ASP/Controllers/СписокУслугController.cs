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
    public class СписокУслугController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: СписокУслуг
        public ActionResult Index()
        {
            var списокУслуг = db.СписокУслуг.Include(с => с.Заказ).Include(с => с.Услуга);
            return View(списокУслуг.ToList());
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
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки");
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
                return RedirectToAction("Index");
            }

            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки", списокУслуг.КодЗаказа);
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
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки", списокУслуг.КодЗаказа);
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
                return RedirectToAction("Index");
            }
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки", списокУслуг.КодЗаказа);
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
