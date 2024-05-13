using System.ComponentModel.DataAnnotations;
public class ProductWithAverageRating : Product 
{
    public short Rating { get; set; } //TODO: Compute average
}
