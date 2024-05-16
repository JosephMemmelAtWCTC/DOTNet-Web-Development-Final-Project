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

        public Product Get(int id) => _dataContext.Products.FirstOrDefault(p => p.ProductId == id);

        [HttpGet, Route("api/productWithRating/{id}")]
        public ProductWithAverageRating GetProductWithRating(int id){
            Product product = _dataContext.Products.FirstOrDefault(p => p.ProductId == id);
            double averageRating = _dataContext.Reviews.Where(r => r.ProductId == id).Average(r => r.Rating);

            return new ProductWithAverageRating{
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                QuantityPerUnit = product.QuantityPerUnit,
                UnitPrice = product.UnitPrice,
                UnitsInStock = product.UnitsInStock,
                UnitsOnOrder = product.UnitsOnOrder,
                ReorderLevel = product.ReorderLevel,
                Discontinued = product.Discontinued,
                CategoryId = product.CategoryId,
                Category = product.Category,
                AverageRating = averageRating,
            };
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


        [HttpGet, Route("api/category/{CategoryId}/productWithAverageReview/discontinued/{discontinued}")]
        public IEnumerable<ProductWithAverageRating> GetByCategoryProductWithRating(int CategoryId, bool discontinued){
            return _dataContext.Products.Where(p => p.CategoryId == CategoryId && p.Discontinued == discontinued).OrderBy(p => p.ProductName)//.Where(p=> p.ProductId == 1)
            // https://learn.microsoft.com/en-us/dotnet/api/system.linq.enumerable.select?view=net-8.0
                .Select(productWithoutRatingForPassthrough => new ProductWithAverageRating{
                    ProductId = productWithoutRatingForPassthrough.ProductId,
                    ProductName = productWithoutRatingForPassthrough.ProductName,
                    QuantityPerUnit = productWithoutRatingForPassthrough.QuantityPerUnit,
                    UnitPrice = productWithoutRatingForPassthrough.UnitPrice,
                    UnitsInStock = productWithoutRatingForPassthrough.UnitsInStock,
                    UnitsOnOrder = productWithoutRatingForPassthrough.UnitsOnOrder,
                    ReorderLevel = productWithoutRatingForPassthrough.ReorderLevel,
                    Discontinued = productWithoutRatingForPassthrough.Discontinued,
                    CategoryId = productWithoutRatingForPassthrough.CategoryId,
                    Category = productWithoutRatingForPassthrough.Category,

                    AverageRating = _dataContext.Reviews.Where(r => r.ProductId == productWithoutRatingForPassthrough.ProductId).Any() ? _dataContext.Reviews.Where(r => r.ProductId == productWithoutRatingForPassthrough.ProductId).Average(r => r.Rating) : -1,
                });
                // Can't use new {} when returning direct new ProductWithAverageRating(){} as "CS0834 - A lambda expression with a statement body cannot be converted to an expression tree."
        }

        [HttpGet, Route("api/category")]
        // returns all categories
        public IEnumerable<Category> GetCategory() => _dataContext.Categories.Include("Products").OrderBy(c => c.CategoryName);

        [HttpPost, Route("api/addtocart")]
        // adds a row to the cartitem table
        public CartItem Post([FromBody] CartItemJSON cartItem) => _dataContext.AddToCart(cartItem);


        // Get all reviews related to the product
        [HttpGet, Route("api/product/reviews/{ProductId}")]
        public IEnumerable<Review> GetReviewsByProduct(int ProductId) => _dataContext.Reviews.Include(r => r.Product).Include(r => r.Customer).Where(r => r.ProductId == ProductId);
        [HttpGet, Route("api/product/averageReview/{ProductId}")]
        public Double GetAverageReviewByProduct(int ProductId) => _dataContext.Reviews.Where(r => r.ProductId == ProductId).Average(r => r.Rating);
    }
}