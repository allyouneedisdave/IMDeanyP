using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models
{
    [Table("REVIEW")]
    public class Review
    {
        [Column("review_id")]
        public int ReviewID { get; set; }

        [Required]
        [Column("film_id")]
        [Display(Name = "Film")]
        public int FilmID { get; set; }

        [Required]
        [Column("review_uname")]
        [Display(Name = "Username")]
        public string ReviewUname { get; set; }

        [Required]
        [Column("review_content")]
        [Display(Name = "Review Content")]
        [DataType(DataType.MultilineText)]
        public string ReviewContent { get; set; }

        [Required]
        [Column("review_rating")]
        [Display(Name = "Rating")]
        public int ReviewRating { get; set; }

        public string ReviewContentTrimmed
        {
            get
            {
                //if longer than 100 characters, trim and add ...
                if ((ReviewContent.Length) > 100)
                    return ReviewContent.Substring(0, 100) + "...";
                else
                    return ReviewContent;
            }
        }
    }
}