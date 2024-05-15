using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


// public class ProductController : Controller
public class ProductController(DataContext db, UserManager<AppUser> usrMgr) : Controller

{
  private readonly DataContext _dataContext = db;
  private readonly UserManager<AppUser> _userManager = usrMgr;

  // this controller depends on the NorthwindRepository
  // private DataContext _dataContext;
  // public ProductController(DataContext db) => _dataContext = db;
  public IActionResult Category() => View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  // public IActionResult Index(int id) => View(_dataContext.Products.Where(p => p.CategoryId == id && p.Discontinued == false).OrderBy(p => p.ProductName));

  public IActionResult Discounts() => View(_dataContext.Discounts.Where(d => d.EndTime > DateTime.Today).OrderBy(d => d.EndTime));


   public IActionResult Index(int id)
    {
        ViewBag.id = id;
        var products = _dataContext.Products.Where(p => p.CategoryId == id && !p.Discontinued).ToList();

        var productRatings = _dataContext.Reviews
            .GroupBy(r => r.ProductId)
            .Select(g => new
            {
                ProductId = g.Key,
                AverageRating = g.Average(r => r.Rating)
            }).ToDictionary(x => x.ProductId, x => x.AverageRating);

        foreach (var product in products)
        {
            if (productRatings.ContainsKey(product.ProductId))
            {
                product.AverageRating = Math.Round(productRatings[product.ProductId], 1);
            }
            else
            {
                product.AverageRating = null;
            }
        }

        ViewBag.Products = products;

        return View(products);
    }

  public IActionResult Review(int id){
    return View(
      new Review
        {
          ReviewAt = DateTime.Now,
          Rating = 0,
          Comment = "",
          ProductId = id,
          CustomerId = _dataContext.Customers.FirstOrDefault(c => c.Email == User.Identity.Name).CustomerId,
        }
    );
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async System.Threading.Tasks.Task<IActionResult> Review(Review newReview)
    {
      if (ModelState.IsValid)
      {
        Review review = new Review
        {
          ReviewAt = DateTime.Now,
          Rating = newReview.Rating,
          Comment = newReview.Comment,
          ProductId = newReview.ProductId,
          CustomerId = newReview.CustomerId,
        };

        _dataContext.AddReview(review);
      }
      return View();
    }


  //   [Authorize(Roles = "northwind-customer")]
  //   public IActionResult Account() => View(_dataContext.Customers.FirstOrDefault(c => c.Email == User.Identity.Name));
  //   [Authorize(Roles = "northwind-customer"), HttpPost, ValidateAntiForgeryToken]
  //   public IActionResult Account(Customer customer)
  //   {
  //       // Edit customer info
  //     _dataContext.EditCustomer(customer);
  //     return RedirectToAction("Index", "Home");
  // }
  //   private void AddErrorsFromResult(IdentityResult result)
  //   {
  //     foreach (IdentityError error in result.Errors)
  //     {
  //       ModelState.AddModelError("", error.Description);
  //     }
  //   }
}