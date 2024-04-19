using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;


public class ProductController : Controller
{
  // this controller depends on the NorthwindRepository
  private DataContext _dataContext;
  public ProductController(DataContext db) => _dataContext = db;
  public IActionResult Category() => View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  // public IActionResult Index(int id) => View(_dataContext.Products.Where(p => p.CategoryId == id && p.Discontinued == false).OrderBy(p => p.ProductName));

  public IActionResult Discounts() => View(_dataContext.Discounts.Where(d => d.EndTime > DateTime.Today).OrderBy(d => d.EndTime));


  public IActionResult Index(int id){
    ViewBag.id = id;
    return View(_dataContext.Categories.OrderBy(c => c.CategoryName));
  }

  public IActionResult Review() => View();
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
          ProductId = 1,
          CustomerId = 1,
        };
        // TODO: Put back equilivent
        // if (_dataContext.Customers.Any(c => c.CompanyName == customer.CompanyName))

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