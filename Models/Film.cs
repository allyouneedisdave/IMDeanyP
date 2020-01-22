using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models
{
    [Table("FILM")]
    public class Film
    {
        
        [Column("film_id")]
        public int FilmID { get; set; }

        [Required]
        [Column("film_title")]
        [DisplayName("Film Title")]
        public string FilmTitle { get; set; }

        [Required]
        [Column("film_genre")]
        [DisplayName("Genre")]
        public string FilmGenre { get; set; }

        [Required]
        [Column("film_desc")]
        [DataType(DataType.MultilineText)]
        [DisplayName("Description")]
        public string FilmDesc { get; set; }   

        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}",
            ApplyFormatInEditMode = true)]

        [Column("film_release_date")]
        [DisplayName("Release Date")]
        public DateTime? FilmReleaseDate { get; set; }

        [Column("film_image")]
        [DisplayName("Image")]
        public string FilmImage { get; set; }

        //trimmed / sample copy of the description for listings
        public string FilmDescTrimmed
        {
            //get only; updates are to FilmDesc
            get
            {
                //if the length of the desc is greater than 100 characters
                if ((FilmDesc.Length) > 100)
                    //get a substring of the first 100 characters followed by ellipses
                    return FilmDesc.Substring(0, 100) + "...";
                else
                    //otherwise return the full description
                    return FilmDesc;
            }
        }



    }
}