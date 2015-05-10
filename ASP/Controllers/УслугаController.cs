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
    public class УслугаController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Услуга
        public ActionResult Index()
        {
            return View(db.Услуга.ToList());
        }

        // GET: Услуга/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Услуга услуга = db.Услуга.Find(id);
            if (услуга == null)
            {
                return HttpNotFound();
            }
            return View(услуга);
        }

        // GET: Услуга/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Услуга/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодУслуги,Наименование,Описание,Стоимость")] Услуга услуга)
        {
            if (ModelState.IsValid)
            {
                db.Услуга.Add(услуга);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(услуга);
        }

        // GET: Услуга/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Услуга услуга = db.Услуга.Find(id);
            if (услуга == null)
            {
                return HttpNotFound();
            }
            return View(услуга);
        }

        // POST: Услуга/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодУслуги,Наименование,Описание,Стоимость")] Услуга услуга)
        {
            if (ModelState.IsValid)
            {
                db.Entry(услуга).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(услуга);
        }

        // GET: Услуга/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Услуга услуга = db.Услуга.Find(id);
            if (услуга == null)
            {
                return HttpNotFound();
            }
            return View(услуга);
        }

        // POST: Услуга/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Услуга услуга = db.Услуга.Find(id);
            db.Услуга.Remove(услуга);
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
