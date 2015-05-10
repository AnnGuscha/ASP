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
    public class СписокКомплектующихController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: СписокКомплектующих
        public ActionResult Index()
        {
            var списокКомплектующих = db.СписокКомплектующих.Include(с => с.Заказ).Include(с => с.Комплектующее);
            return View(списокКомплектующих.ToList());
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

        // GET: СписокКомплектующих/Create
        public ActionResult Create()
        {
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки");
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
                return RedirectToAction("Index");
            }

            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки", списокКомплектующих.КодЗаказа);
            ViewBag.КодКомплектующего = new SelectList(db.Комплектующее, "КодКомплектующего", "Марка", списокКомплектующих.КодКомплектующего);
            return View(списокКомплектующих);
        }

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
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки", списокКомплектующих.КодЗаказа);
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
                return RedirectToAction("Index");
            }
            ViewBag.КодЗаказа = new SelectList(db.Заказ, "КодЗаказа", "Отметки", списокКомплектующих.КодЗаказа);
            ViewBag.КодКомплектующего = new SelectList(db.Комплектующее, "КодКомплектующего", "Марка", списокКомплектующих.КодКомплектующего);
            return View(списокКомплектующих);
        }

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
