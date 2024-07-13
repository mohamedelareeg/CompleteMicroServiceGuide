namespace Invoice.Api.Dtos
{
    public class InvoiceDto
    {
        public string Id { get; set; }
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
    }
}
