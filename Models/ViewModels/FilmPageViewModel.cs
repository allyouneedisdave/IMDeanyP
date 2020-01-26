using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models.ViewModels
{
    public class FilmPageViewModel
    {
        //the film record
        public Film Film;
        //related review records
        public IList<Review> Reviews;
        //related person records linked via acting
        public IList<Person> Actors;
    }
}