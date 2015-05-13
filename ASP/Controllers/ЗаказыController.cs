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
    public class ЗаказыController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Заказы
        public ActionResult Index()
        {
           // var заказы = db.Заказы.Include(з => з.Сотрудник).Include(з => з.Заказчик);
           // return View(заказы.ToList());
            return View(db.Заказы.ToList());
        } 

        // GET: Заказы/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказы заказы = db.Заказы.Find(id);
            if (заказы == null)
            {
                return HttpNotFound();
            }
            return View(заказы);
        }

        // GET: Заказы/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Заказы/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодЗаказа,ДатаЗаказа,ДатаИсполнения,КодЗаказчика,Предоплата,Отметки,Гарантия,КодСотрудника,Стоимость")] Заказы заказы)
        {
            if (ModelState.IsValid)
            {
                db.Заказы.Add(заказы);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(заказы);
        }

        // GET: Заказы/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказы заказы = db.Заказы.Find(id);
            if (заказы == null)
            {
                return HttpNotFound();
            }
            return View(заказы);
        }

        // POST: Заказы/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодЗаказа,ДатаЗаказа,ДатаИсполнения,КодЗаказчика,Предоплата,Отметки,Гарантия,КодСотрудника,Стоимость")] Заказы заказы)
        {
            if (ModelState.IsValid)
            {
                db.Entry(заказы).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(заказы);
        }

        // GET: Заказы/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Заказы заказы = db.Заказы.Find(id);
            if (заказы == null)
            {
                return HttpNotFound();
            }
            return View(заказы);
        }

        // POST: Заказы/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Заказы заказы = db.Заказы.Find(id);
            db.Заказы.Remove(заказы);
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
