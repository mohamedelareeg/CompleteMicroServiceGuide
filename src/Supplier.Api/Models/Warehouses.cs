namespace Supplier.Api.Models
{
    public class Warehouses
    {
        public Guid WarehouseId { get; set; }

        public string Location { get; set; }

        public List<Products>? Products { get; set; }
    }
}
