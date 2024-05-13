using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace Northwind.Controllers
{
    public class APIController(DataContext db) : Controller
    {
        // this controller depends on the NorthwindRepository
        private readonly DataContext _dataContext = db;

        [HttpGet, Route("api/product")]
        // returns all products
        public IEnumerable<Product> Get() => _dataContext.Products.OrderBy(p => p.ProductName);

        [HttpGet, Route("api/product/{id}")]
        // returns specific product

        // .Include("Category")
        public Product Get(int id) => _dataContext.Products.FirstOrDefault(p => p.ProductId == id);

        [HttpGet, Route("api/productWithRating/{id}")]
        public ActionResult<Product> GetProductWithRating(int id)
        {
            var product = _dataContext.Products
                .Where(p => p.ProductId == id)
                .Include(p => p.Category) // Ensure related data like categories is included if needed
                .Select(p => new
                {
                    Product = p,
                    Reviews = _dataContext.Reviews.Where(r => r.ProductId == p.ProductId).ToList() // Fetch reviews here
                })
                .FirstOrDefault();
 
            if (product == null)
                return NotFound(); // Handle the case where the product does not exist

            // Calculate the average rating if reviews are available
            double? averageRating = product.Reviews.Any() ? product.Reviews.Average(r => r.Rating) : null;

            // Map the result back to a Product instance, adding the average rating
            Product result = product.Product;
            result.AverageRating = averageRating.HasValue ? Math.Round(averageRating.Value, 1) : null;

            return Ok(result); // Return the product along with its computed average rating
        }


        [HttpGet, Route("api/product/discontinued/{discontinued}")]
        // returns all products where discontinued = true/false
        public IEnumerable<Product> GetDiscontinued(bool discontinued) => _dataContext.Products.Where(p => p.Discontinued == discontinued).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category/{CategoryId}/product")]
        // returns all products in a specific category
        public IEnumerable<Product> GetByCategory(int CategoryId) => _dataContext.Products.Where(p => p.CategoryId == CategoryId).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category/{CategoryId}/product/discontinued/{discontinued}")]
        // returns all products in a specific category where discontinued = true/false
        public IEnumerable<Product> GetByCategoryDiscontinued(int CategoryId, bool discontinued) => _dataContext.Products.Where(p => p.CategoryId == CategoryId && p.Discontinued == discontinued).OrderBy(p => p.ProductName);

        [HttpGet, Route("api/category")]
        // returns all categories
        public IEnumerable<Category> GetCategory() => _dataContext.Categories.Include("Products").OrderBy(c => c.CategoryName);

        [HttpPost, Route("api/addtocart")]
        // adds a row to the cartitem table
        public CartItem Post([FromBody] CartItemJSON cartItem) => _dataContext.AddToCart(cartItem);
    }
}
