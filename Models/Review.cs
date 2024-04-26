using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Review
{
    // Unique to Rating
    public int ReviewId { get; set; }
    public DateTime ReviewAt { get; set; }

    [Range(1, 5, ErrorMessage = "Value for {0} must be between {1} and {2}.")] // Limit to 1-5 for stars
    public byte Rating { get; set; } // unsigned 8-bit range is (0-255)

    public String Comment { get; set; }


    // Linking / refrenceing to other data
    public int ProductId { get; set; }
    public Product Product { get; set; }

    public int CustomerId { get; set; }
    public Customer Customer { get; set; }

}
