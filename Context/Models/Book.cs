using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Context.Models
{
    public class Book
    {
        [Key]
        public int BookId { get; set; }

        [Required]
        [Display(Name = "Book Title")]
        public string Title { get; set; }

        [Required]
        public string AuthorFirstName { get; set; }

        [Required]
        public string AuthorLastName { get; set; }

        [Required]
        public string Country { get; set; }

        [Required]
        public string HistoricalLink { get; set; }

        public string AuthorInfo { get; set; }

        public string BookInfo { get; set; }

        public string CountryInfo { get; set; }

        public string HistoricalInfo { get; set; }

        public virtual ICollection<UserBook> UserBooks { get; set; }



    }
}
