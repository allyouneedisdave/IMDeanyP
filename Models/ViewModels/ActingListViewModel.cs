using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models.ViewModels
{
    public class ActingListViewModel
    {
        //the Acting model represented here
        public Acting ActingCredit;

        //here we have the linked Person to access their data
        public Person Actor;

        //here we have the linked film to access its data
        public Film Film;

    }
}