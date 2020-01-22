using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models
{
    [Table("PERSON")]
    public class Person
    {
        [Column("person_id")]
        public int PersonId { get; set; }

        [Required]
        [Column("person_fname")]
        [Display(Name ="First Name")]
        public string PersonFname { get; set; }

        [Required]
        [Column("person_sname")]
        [Display(Name ="Surname")]
        public string PersonSname { get; set; }

        [Required]
        [Column("person_desc")]
        [Display(Name ="Bio")]
        [DataType(DataType.MultilineText)]
        public string PersonDesc { get; set; }

        [Column("person_image")]
        [Display(Name ="Profile Image")]
        public string PersonImage { get; set; }

        //trimmed / sample copy of the description for listings

        public string PersonDescTrimmed
        {
            //get only; updates are to FilmDesc
            get
            {
                //if the length of the desc is greater than 100 characters
                if ((PersonDesc.Length) > 100)
                    //get a substring of the first 100 characters followed by ellipses
                    return PersonDesc.Substring(0, 100) + "...";
                else
                    return PersonDesc;
            }
        }
    }
}