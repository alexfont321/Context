using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Context.Models
{
    public class UserBook
    {

        [Key]
        public string UserBookId { get; set; }

        [Required]
        public int BookId { get; set; }

        [Required]
        public int UserId { get; set; }

        public Book Book { get; set; }

        public ApplicationUser User { get; set; }

    }
}
