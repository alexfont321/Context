using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Context.Models
{
    public class BooksWithUserViewModel
    {
        public ApplicationUser User { get; set; }

        public IEnumerable<Book> Books { get; set; }
    }
}
