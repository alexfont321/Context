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

        [Display(Name = "Author Information")]
        public string AuthorInfo { get; set; }

        [Display(Name = "Book Information")]
        public string BookInfo { get; set; }

        [Display(Name = "Country Information")]
        public string CountryInfo { get; set; }

        [Display(Name = "Historical Information")]
        public string HistoricalInfo { get; set; }

        public virtual ICollection<UserBook> UserBooks { get; set; }



    }
}
