using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IMDeanyP.Models;
using IMDeanyP.Models.ViewModels;

namespace IMDeanyP.Controllers
{
    public class ActingsController : Controller
    {
        private DBContext db = new DBContext();

        // GET: Actings
        public ActionResult Index()
        {
            //create a list for the view model to link Film and Person
            List<ActingListViewModel> ActingList = new List<ActingListViewModel>();

            //separate list for the Acting credits to get the keys
            List<Acting> actingCredits;

            //populate the Actings list by selecting all records
            //from the db context
            actingCredits = db.Actings.ToList();

            //loop through each record to get the foreign keys
            //then populate the view model with the relevant
            //film / Person
            foreach (Acting a in actingCredits)
            {
                //match the ID between Acting and Film - store the single record in 'film'
                Film film = db.Films.Where(x => x.FilmID == a.FilmId).Single();

                //match the ID between Acting and Film - store the single record in 'actor'
                Person actor = db.Persons.Where(x => x.PersonId == a.PersonId).Single();

                //new ActingListViewModel object to then add to the list
                ActingListViewModel toAdd = new ActingListViewModel();

                toAdd.ActingCredit = a; //get the acting credit record
                toAdd.Film = film; //get the film record
                toAdd.Actor = actor; //get the person record (as actor)

                //add tp the ActingList (list of ViewModel objects)
                ActingList.Add(toAdd);
    
            }
            //send the ActingListViewModel List to the View for display
            return View(ActingList);
        }

        // GET: Actings/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acting acting = db.Actings.Find(id);
            if (acting == null)
            {
                return HttpNotFound();
            }
            return View(acting);
        }

        // GET: Actings/Create
        public ActionResult Create(int FilmId = 0, int PersonId = 0)
        {
            //FILMS-------------------------------------------------------
            //from the Films model DbSet
            //select all columns from the database
            //orderby the film title
            var filmQuery = from m in db.Films
                            orderby m.FilmTitle
                            select m;
            //if no id set
            if (FilmId == 0)
                //construct full films dropdown list without preselection
                //do so from the query results and display the FilmTitle
                //store in FilmID in the ViewBag
                ViewBag.FilmId = new SelectList(filmQuery, "FilmID", "FilmTitle", null);
            else
                //construct as above but with the FilmID preselected
                ViewBag.FilmId = new SelectList(filmQuery, "FilmID", "FilmTitle", FilmId);

            //PERSONS ---------------------------------------------------------------------
            //from the Persons model DbSet
            //select the fname and sname as a new field called Name
            //add the person id - order by the sname
            var personsQuery = from p in db.Persons
                               orderby p.PersonSname
                               select new
                               {
                                   Name = p.PersonFname + " " + p.PersonSname,
                                   p.PersonId
                               };
            //if no id set
            if (PersonId == 0)
                //construct full films dropdown list without preselection
                //do so from the query results and display the Name (combined above)
                //store in FilmId in the ViewBag
                ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", null);
            else
                //construct as above but with the PersonId preselected
                ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", PersonId);

            //generate the view
            return View();
        }

        // POST: Actings/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ActingId,PersonId,FilmId")] Acting acting)
        {
            if (ModelState.IsValid)
            {
                db.Actings.Add(acting);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(acting);
        }

        // GET: Actings/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acting acting = db.Actings.Find(id);
            if (acting == null)
            {
                return HttpNotFound();
            }

            //code to generate dropdowns
            //FILMS --------------------------------------------------------------------
            //from the Films model DbSet
            //select all columns from the database
            //orderby the film title
            var filmQuery = from m in db.Films
                            orderby m.FilmTitle
                            select m;
            //construct full films dropdown list preselected with the foreign key
            //do so from the query results and display the FilmTitle
            //store in FilmID in the ViewBag
            ViewBag.FilmId = new SelectList(filmQuery, "FilmID", "FilmTitle", acting.FilmId);

            //PERSONS ------------------------------------------------------------------
            //from the Persons model DbSet
            //select the fname and sname as a new field called Name
            //and the person id - order by the sname
            var personsQuery = from p in db.Persons
                               orderby p.PersonSname
                               select new
                               {
                                   Name = p.PersonFname + " " + p.PersonSname,
                                   p.PersonId
                               };
            //construct full films dropdown list preselected with the foreign key
            //do so from the query results and display the Name (combined above)
            //store in FilmId in the ViewBag
            ViewBag.PersonId = new SelectList(personsQuery, "PersonId", "Name", acting.PersonId);

            return View(acting);

        }

        // POST: Actings/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ActingId,PersonId,FilmId")] Acting acting)
        {
            if (ModelState.IsValid)
            {
                db.Entry(acting).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(acting);
        }

        // GET: Actings/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Acting acting = db.Actings.Find(id);
            if (acting == null)
            {
                return HttpNotFound();
            }
            return View(acting);
        }

        // POST: Actings/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Acting acting = db.Actings.Find(id);
            db.Actings.Remove(acting);
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
