using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class OrderDetails
{
    public int OrderDetailsId { get; set; }
    [Required]
    public int OrderId { get; set; }
    [Required]
    public int ProductId { get; set; }
    [Required]
    // [DataType(DataType.Currency)]
    [DisplayFormat(DataFormatString = "{0:C2}")]
    public decimal UnitPrice { get; set; }
    [Required]
    // [Column(TypeName = "decimal(3,2)")]
    public short Quantity { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Discount { get; set; }

    public Order Order { get; set; }
    public Product Product { get; set; }
}
