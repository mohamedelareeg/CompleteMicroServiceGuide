using Invoice.Api.Dtos;
using Invoice.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Invoice.Api.Services
{
    public class InvoiceService : IInvoiceService
    {
        private readonly AppDbContext _dbContext;

        public InvoiceService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<string> CreateInvoiceAsync(CreateInvoiceDto dto)
        {
            var invoice = new Invoices
            {
                InvoiceNumber = dto.InvoiceNumber,
                InvoiceDate = dto.InvoiceDate,
                TotalAmount = dto.TotalAmount
            };

            _dbContext.Invoices.Add(invoice);
            await _dbContext.SaveChangesAsync();
            return invoice.Id.ToString();
        }

        public async Task<List<InvoiceDto>> GetInvoicesAsync()
        {
            var invoices = await _dbContext.Invoices.ToListAsync();
            return invoices.Select(i => new InvoiceDto
            {
                Id = i.Id.ToString(),
                InvoiceNumber = i.InvoiceNumber,
                InvoiceDate = i.InvoiceDate,
                TotalAmount = i.TotalAmount
            }).ToList();
        }
    }
}
