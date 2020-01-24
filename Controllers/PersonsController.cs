using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IMDeanyP.Models;

namespace IMDeanyP.Controllers
{
    public class PersonsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Persons
        public ActionResult Index()
        {
            return View(db.Persons.ToList());
        }

        // GET: Persons/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // GET: Persons/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Persons/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PersonId,PersonFname,PersonSname,PersonDesc,PersonImage")] Person person, HttpPostedFileBase upload)
        {
            //if we have valid data in the form
            if (ModelState.IsValid)
            {
                //check to see if a file has been uploaded
                if (upload != null && upload.ContentLength > 0)
                {
                    //check to see if valid MIME type (JPG / PNG or GIF images)
                    if (upload.ContentType == "image/jpeg" ||
                        upload.ContentType == "image/jpg" ||
                        upload.ContentType == "image/gif" ||
                        upload.ContentType == "image/png")
                    {
                        //construct a path to put the file in an Images subfolder in Content
                        string path = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(upload.FileName));
                        //save the file to that path location
                        upload.SaveAs(path);

                        //store the relative path to the image in the database
                        person.PersonImage = "~/Content/Images/" + Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //construct a message that can be displayed in the view
                        ViewBag.Message = "Not valid image format";
                    }
                }
                //add the person to the database and save
                db.Persons.Add(person);
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);

        }

        // GET: Persons/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Persons/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PersonId,PersonFname,PersonSname,PersonDesc,PersonImage")] Person person, HttpPostedFileBase upload)
        {
            if (ModelState.IsValid)
            {
                //check to see if a file has been uploaded
                if (upload != null && upload.ContentLength > 0)
                {
                    //check to see if valid MIME type (JPG / PNG or GIF images)
                    if (upload.ContentType == "image/jpeg" ||
                        upload.ContentType == "image/jpg" ||
                        upload.ContentType == "image/gif" ||
                        upload.ContentType == "image/png")
                    {
                        //construct a path to put the file in an Images subfolder in Content
                        string path = Path.Combine(Server.MapPath("~/Content/Images"), Path.GetFileName(upload.FileName));

                        //save the file to that path location
                        upload.SaveAs(path);

                        //store the relative path to the image in the database
                        person.PersonImage = "~/Content/Images/" + Path.GetFileName(upload.FileName);

                    }
                    else
                    {
                        //construct a message that can be displayed in the view
                        ViewBag.Message = "Not valid image format";
                    }
                }

                db.Entry(person).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(person);
        }

        // GET: Persons/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Person person = db.Persons.Find(id);
            if (person == null)
            {
                return HttpNotFound();
            }
            return View(person);
        }

        // POST: Persons/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Person person = db.Persons.Find(id);
            db.Persons.Remove(person);
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
