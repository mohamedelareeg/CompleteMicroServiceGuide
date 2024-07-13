using Invoice.Api.Dtos;
using Invoice.Api.Services;
using Microsoft.AspNetCore.Mvc;

namespace Invoice.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly IInvoiceService _invoiceService;

        public InvoiceController(IInvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateInvoice([FromBody] CreateInvoiceDto dto)
        {
            var invoiceId = await _invoiceService.CreateInvoiceAsync(dto);
            return Ok(new { InvoiceId = invoiceId, Message = "Invoice created successfully." });
        }

        [HttpGet]
        public async Task<IActionResult> GetInvoices()
        {
            var invoices = await _invoiceService.GetInvoicesAsync();
            return Ok(invoices);
        }

    }
}
