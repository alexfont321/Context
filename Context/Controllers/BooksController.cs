using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Context.Data;
using Context.Models;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Context.Controllers
{
    public class BooksController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public BooksController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;

        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);


        static HttpClient client = new HttpClient();


        // GET: Books
        public async Task<IActionResult> Index()
        {
            BooksWithUserViewModel BU = new BooksWithUserViewModel();

            var user = await GetCurrentUserAsync();

            BU.User = user;
            BU.Books = await _context.Books.OrderByDescending(b => b.BookId).Take(10).ToListAsync();

            return View(BU);

            //return View(await _context.Books.OrderByDescending(b => b.BookId).Take(10).ToListAsync());
        }

        // GET: Books/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // GET: Books/Create
        public IActionResult Create()
        {

            return View();
        }

        // POST: Books/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("BookId,Title,AuthorFirstName,AuthorLastName,Country,HistoricalLink,AuthorInfo,BookInfo,CountryInfo,HistoricalInfo")] Book book)
        {

           // ModelState.Remove("AuthorInfo");
           // ModelState.Remove("BookInfo");

            if (ModelState.IsValid)
            {
                book.AuthorInfo = await GetRequest(WikiString($"{book.AuthorFirstName}_{book.AuthorLastName}"));
                book.BookInfo = await GetRequest(WikiString($"{book.Title.Replace(" ", "_")}"));
                book.CountryInfo = await GetRequest(WikiHistoryString($"{book.Country.Replace(" ", "_")}"));
                book.HistoricalInfo = await GetRequest(WikiString($"{book.HistoricalLink.Substring(30)}"));
                _context.Add(book);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books.FindAsync(id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        // POST: Books/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("BookId,Title,AuthorFirstName,AuthorLastName,Country,HistoricalLink,AuthorInfo,BookInfo,CountryInfo,HistoricalInfo")] Book book)
        {
            if (id != book.BookId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(book);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BookExists(book.BookId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(book);
        }

        // GET: Books/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _context.Books
                .FirstOrDefaultAsync(m => m.BookId == id);
            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }

        // POST: Books/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _context.Books.FindAsync(id);
            _context.Books.Remove(book);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        //Method to save book as a UserBook
        [Authorize]
        public async Task<IActionResult> SaveUserBook(int id)
        {
            Book bookToAdd = await _context.Books.SingleOrDefaultAsync(b => b.BookId == id);
            var user = await GetCurrentUserAsync();

            UserBook ub = new UserBook();
            ub.BookId = bookToAdd.BookId;
            ub.UserId = user.Id;
            _context.Add(ub);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "UserBooks");



        }

        private bool BookExists(int id)
        {
            return _context.Books.Any(e => e.BookId == id);
        }

        public string WikiString(string s)
        {
            string url = $@"https://en.wikipedia.org/w/api.php?action=query&titles={s}&prop=extracts&exintro=&explaintext=&iformat=json&format=json";
            return url;
        }

        public string WikiHistoryString(string s)
        {
            string url = $@"https://en.wikipedia.org/w/api.php?action=query&titles=History_of_{s}&prop=extracts&exintro=&explaintext=&iformat=json&format=json";
            return url;
        }

        public async Task<string> GetRequest(string url)
        {
            
            using (HttpClient client = new HttpClient())
            {
                using (HttpResponseMessage response = await client.GetAsync(url)) 
                {
                    using( HttpContent content = response.Content)
                    {
                        string result = await content.ReadAsStringAsync();
                        var parsedResponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(result);
                        var foo = ((JObject)parsedResponse["query"]).Value<JObject>("pages").First.First;
                        var bar = foo.ToObject<Dictionary<string, object>>();
                        var baz = bar["extract"].ToString();
                        return baz;

                    }
                }
            }
        }
    }
}
