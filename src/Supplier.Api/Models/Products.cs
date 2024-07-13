namespace Supplier.Api.Models
{
    public class Products
    {
        public Guid ProductId { get; set; }

        public string Name { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
