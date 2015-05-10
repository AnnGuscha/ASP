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
    public class КомплектующееController : Controller
    {
        private КомпьютернаяФирмаEntities db = new КомпьютернаяФирмаEntities();

        // GET: Комплектующее
        public ActionResult Index(string KomplMarkFind = "")
        {
            //var комплектующее = db.Комплектующее.Include(к => к.ВидКомплектующих);
            var комплектующее = from k in db.Комплектующее
                where k.Марка.StartsWith(KomplMarkFind)
                select k;
            return View(комплектующее.ToList());
        }
        public ActionResult Home(string KomplMarkFind = "")
        {
            //var комплектующее = db.Комплектующее.Include(к => к.ВидКомплектующих);
            var комплектующее = from k in db.Комплектующее
                                where k.Марка.StartsWith(KomplMarkFind)
                                select k; 
            return View(комплектующее.ToList());

        }
        
        // GET: Комплектующее/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Комплектующее комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            return View(комплектующее);
        }

        // GET: Комплектующее/Create
        public ActionResult Create()
        {
            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование");
            return View();
        }

        // POST: Комплектующее/Create
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "КодКомплектующего,КодВида,Марка,ФирмаПроизводитель,СтранаПроизводитель,ДатаВыпуска,Характеристики,СрокГарантии,Описание,Цена")] Комплектующее комплектующее)
        {
            if (ModelState.IsValid)
            {
                db.Комплектующее.Add(комплектующее);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование", комплектующее.КодВида);
            return View(комплектующее);
        }

        // GET: Комплектующее/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Комплектующее комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование", комплектующее.КодВида);
            return View(комплектующее);
        }

        // POST: Комплектующее/Edit/5
        // Чтобы защититься от атак чрезмерной передачи данных, включите определенные свойства, для которых следует установить привязку. Дополнительные 
        // сведения см. в статье http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "КодКомплектующего,КодВида,Марка,ФирмаПроизводитель,СтранаПроизводитель,ДатаВыпуска,Характеристики,СрокГарантии,Описание,Цена")] Комплектующее комплектующее)
        {
            if (ModelState.IsValid)
            {
                db.Entry(комплектующее).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.КодВида = new SelectList(db.ВидКомплектующих, "КодВида", "Наименование", комплектующее.КодВида);
            return View(комплектующее);
        }

        // GET: Комплектующее/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Комплектующее комплектующее = db.Комплектующее.Find(id);
            if (комплектующее == null)
            {
                return HttpNotFound();
            }
            return View(комплектующее);
        }

        // POST: Комплектующее/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Комплектующее комплектующее = db.Комплектующее.Find(id);
            db.Комплектующее.Remove(комплектующее);
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
