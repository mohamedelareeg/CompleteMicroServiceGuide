namespace Invoice.Api.Dtos
{
    public class CreateInvoiceDto
    {
        public string InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public double TotalAmount { get; set; }
    }
}
