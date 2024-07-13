using Invoice.Api.Dtos;

namespace Invoice.Api.Services
{
    public interface IInvoiceService
    {
        Task<string> CreateInvoiceAsync(CreateInvoiceDto dto);
        Task<List<InvoiceDto>> GetInvoicesAsync();
    }
}
