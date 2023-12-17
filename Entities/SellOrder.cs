using System.ComponentModel.DataAnnotations;

namespace Entities
{
    public class SellOrder
    {
        [Key]
        public Guid SellOrderID { get; set; }
        [Required(ErrorMessage = "Stock Symbol can't be blank")]
        public string? StockSymbol { get; set; }
        [Required(ErrorMessage = "Stock Name can't be blank")]
        public string? StockName { get;set; }
        public DateTime DateAndTimeOfOrder { get; set; }
        [Range(1, 100000,ErrorMessage = "Can sell maximum of 100000 shares in single order, Minimum is 1.")]
        public uint Quantity { get; set; } 
        [Range(1, 10000,ErrorMessage = "The maximum selling price of stock is 10000, Minimum is 1.")]
        public double Price { get; set; }
    }
}
