// NOTE: [Required] labels removed from fields that ended up already having a NULL value.

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Order
{
    public int OrderId { get; set; }
    [Required]
    public int CustomerId { get; set; }
    [Required]
    public int EmployeeId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime RequiredDate { get; set; }

    public DateTime? ShippedDate { get; set; }
    public int ShipVia { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Freight { get; set; }
    [Required]
    public string ShipName { get; set; }
    [Required]
    public string ShipAddress { get; set; }
    [Required]
    public string ShipCity { get; set; }
    public string ShipRegion { get; set; }
    public string ShipPostalCode { get; set; }
    public string ShipCountry { get; set; }

    public ICollection<OrderDetails> OrderDetails { get; set; }
    public Customer Customer { get; set; }
}
