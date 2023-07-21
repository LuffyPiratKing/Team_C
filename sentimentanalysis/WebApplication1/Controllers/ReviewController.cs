using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class ReviewController : Controller
    {
        private readonly AnalysisDbContext _context;
        public ReviewController(AnalysisDbContext context)
        {
            _context = context;
        }


        // GET: ReviewController
        public ActionResult Index()
        {
            List<Review> reviews = _context.reviews.ToList();
            if (reviews == null)
            {
                return View();
            }
            else
            {
            return View(reviews);
            }
        }

        // GET: ReviewController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ReviewController/Create
        public ActionResult Create()
        {
            return View("Index");
        }

        // POST: ReviewController/Create
        [HttpPost]
        public ActionResult Create(Review review)
        {
            try
            {
                string name = review.Name;
                string image = review.Image;
                string comments = review.Comments;
       
                return View("Index");
            }
            catch
            {
                return View();
            }
        }

        // GET: ReviewController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ReviewController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ReviewController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ReviewController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
