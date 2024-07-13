namespace Invoice.Api.Models
{
    public class Invoices
    {
        public int Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
    }
}
