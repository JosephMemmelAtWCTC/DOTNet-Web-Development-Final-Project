using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
public class CustomerOrdersAndOrderDetails
{

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }
    public IEnumerable<Order> Orders { get; set; }
    public IEnumerable<OrderDetails> OrderDetails { get; set; }
    public IEnumerable<Review> LeftReviews { get; set; }
    
    // public IEnumerable<Product> Collated { get; set; }

}
