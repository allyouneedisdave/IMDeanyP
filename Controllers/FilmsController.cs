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
using IMDeanyP.Models.ViewModels;
using PagedList;

namespace IMDeanyP.Controllers
{
    public class FilmsController : Controller
    {
        private DBContext db = new DBContext();

        //to be accessed via AJAX - autocomplete jQuery UI plugin
        public ActionResult Search(string term)
        {
            //select all the films in the db
            //and get the id and title only
            //id and label used for autocomplete functionality
            var films = from f in db.Films
                        select new
                        {
                            id = f.FilmID,
                            label = f.FilmTitle
                        };

            //now check the searchstring given for any matches in title
            films = films.Where(f => f.label.Contains(term));

            //convert to and return the JSON for the search UI
            return Json(films, JsonRequestBehavior.AllowGet);
        }


        // GET: Films
        public ActionResult Index(string sortOrder, string searchString, string currentFilter, int? page)
        {
            //for the viewbag to keep a note of current sort order
            ViewBag.CurrentSort = sortOrder;

            //add a new value to the Viewbag to retain current sort order
            //check if the sortOrder param is empty - if so we'll set the next choice
            //to title_desc (order by title descending) otherwise empty string
            //lets us construct a toggle link for the alternative
            ViewBag.TitleSortParam = String.IsNullOrEmpty(sortOrder) ? "title_desc" : "";

            //if there is a search string
            if(searchString != null)
            {
                //set page as 1
                page = 1;
            }
            else
            {
                //if no search string, set to the current filter
                searchString = currentFilter;
            }

            //the current filter is now the search string - note kept in view
            ViewBag.CurrentFilter = searchString;

            //select all the films in the db
            var films = from f in db.Films select f;

            //check if the search string is not empty
            if (!String.IsNullOrEmpty(searchString))
            {
                //if we have a searchterm, then select where the title contains it
                //analogous to LIKE %term% in SQL
                films = films.Where(f => f.FilmTitle.Contains(searchString));
            }

            //check the sortOrder param
            switch (sortOrder)
            {
                case "title_desc":
                    //order by title descending
                    films = films.OrderByDescending(f => f.FilmTitle);
                    break;
                default:
                    //order by title ascending
                    films = films.OrderBy(f => f.FilmTitle);
                    break;
            }

            //how many records per page (could also be a param...)
            int pageSize = 3;
            //if page is null set to 1 otherwise keep page value
            int pageNumber = (page ?? 1);

            //send the updated films list to the view
            return View(films.ToPagedList(pageNumber, pageSize));
        }

        // GET: Films/Details/5
        public ActionResult Details(int? id)
       {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }

            //new view model object
            FilmPageViewModel filmPage = new FilmPageViewModel();

            //get the current film and assign to view model
            filmPage.Film = film;
            //populate the Reviews list for the view model by matching all reviews
            //where the film id matches
            filmPage.Reviews = db.Reviews.Where(x => x.FilmID == film.FilmID).ToList();

            //for actors, first we need to get all related records in the join table
            IList<Acting> actorLinks = db.Actings.Where(x => x.FilmId == film.FilmID).ToList();
            //then we'll construct a list of the person records to match
            IList<Person> actors = new List<Person>();
            //here we loop through the acting records in the join table
            foreach (Acting a in actorLinks)
            {
                //and add to the list of actors the matching person record for each
                actors.Add(db.Persons.Where(x => x.PersonId == a.PersonId).Single());
            }
            //once populated, we can assign the list of people as actors to the view model
            filmPage.Actors = actors;
            //here we will use a LINQ query to get the average review score for reviews
            //related to this film - the additional ? symbols are if there is a null result
            //if so we set to 0
            ViewBag.AverageReview =
                db.Reviews.Where(x => x.FilmID == film.FilmID)
                    .Average(x => (double?)x.ReviewRating) ?? 0;

            //return the view model to use
            return View(filmPage);
        }

        // GET: Films/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Films/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(
            [Bind(Include = "FilmID,FilmTitle,FilmGenre," +
            "FilmDesc,FilmReleaseDate,FilmImage")] Film film,
            HttpPostedFileBase upload)
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
                        film.FilmImage = "~/Content/Images/" + Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //construct a message that can be displayed in the view
                        ViewBag.Message = "Not valid image format";
                    }
                }
                //add the film to the database and save
                db.Films.Add(film);
                db.SaveChanges();
                //redirect to Index
                return RedirectToAction("Index");
            }
            return View(film);
        }

        // GET: Films/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "FilmID,FilmTitle,FilmGenre,FilmDesc,FilmReleaseDate,FilmImage")] Film film, HttpPostedFileBase upload)
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
                        string path = Path.Combine(Server.MapPath("~/Content/Images"),
                                      Path.GetFileName(upload.FileName));
                        //save the file to that path location
                        upload.SaveAs(path);
                        //store the relative path to the image in the database
                        film.FilmImage = "~/Content/Images/" +
                            Path.GetFileName(upload.FileName);
                    }
                    else
                    {
                        //constuct a message that can be displayed in the view
                        ViewBag.Message = "Not valid image format";
                    }
                }
                db.Entry(film).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(film);
        }


        // GET: Films/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Film film = db.Films.Find(id);
            if (film == null)
            {
                return HttpNotFound();
            }
            return View(film);
        }

        // POST: Films/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Film film = db.Films.Find(id);
            db.Films.Remove(film);
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
