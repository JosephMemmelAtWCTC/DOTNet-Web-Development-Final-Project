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


  public IActionResult Index(int id){
    ViewBag.id = id;
    return View(_dataContext.Categories.OrderBy(c => c.CategoryName));
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
      return RedirectToAction("Purchases", "Customer");
    }

}