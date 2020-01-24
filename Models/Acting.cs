using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace IMDeanyP.Models
{
    [Table("ACTING")]
    public class Acting
    {
        [Column("acting_id")]
        public int ActingId { get; set; }

        [Required]
        [Display(Name = "Actor Selection")]
        [Column("person_id")]
        public int PersonId { get; set; }

        [Required]
        [Display(Name = "Film Selection")]
        [Column("film_id")]
        public int FilmId { get; set; }

    }
}