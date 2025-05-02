using InvoiceAPI.Models;
using InvoiceAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InvoiceController : ControllerBase
    {
        private readonly InvoiceService _invoiceService;

        public InvoiceController(InvoiceService invoiceService)
        {
            _invoiceService = invoiceService;
        }

        // GET: api/invoice (tüm faturaları alma)
        [HttpGet]
        public async Task<ActionResult<InvoiceResponseDto>> GetInvoice(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate,
            [FromQuery] string searchQuery = "",
            [FromQuery] string sort = "", 
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _invoiceService.GetInvoicesAsync(startDate, endDate, searchQuery, sort, pageNumber, pageSize);

            var dtos = result.Invoices.Select(inv => new InvoiceDto
            {
                InvoiceId     = inv.InvoiceId,
                CustomerId    = inv.CustomerId,
                InvoiceNumber = inv.InvoiceNumber,
                InvoiceDate   = inv.InvoiceDate,
                TotalAmount   = inv.TotalAmount,
                UserId        = inv.UserId,
                RecordDate    = inv.RecordDate
            }).ToList();

            var response = new InvoiceResponseDto
            {
                Invoices = dtos,
                TotalCount = result.TotalCount
            };

            return Ok(response);
        }





        // POST: api/invoice (fatura oluşturma)
        [HttpPost]
        public async Task<ActionResult<InvoiceDto>> PostInvoice([FromBody] InvoiceDto dto)
        {
            if (dto == null)
                return BadRequest("Invoice data is null");

            var invoice = new Invoice
            {
                CustomerId    = dto.CustomerId,
                InvoiceNumber = dto.InvoiceNumber,
                InvoiceDate   = dto.InvoiceDate,
                TotalAmount   = dto.TotalAmount,
                UserId        = dto.UserId,
                RecordDate    = dto.RecordDate
            };

            var added = await _invoiceService.AddInvoiceAsync(invoice);

            var resultDto = new InvoiceDto
            {
                InvoiceId     = added.InvoiceId,
                CustomerId    = added.CustomerId,
                InvoiceNumber = added.InvoiceNumber,
                InvoiceDate   = added.InvoiceDate,
                TotalAmount   = added.TotalAmount,
                UserId        = added.UserId,
                RecordDate    = added.RecordDate
            };

            return CreatedAtAction(nameof(GetInvoice), new { id = resultDto.InvoiceId }, resultDto);
        }

        // PUT: api/invoice/5 (fatura güncelleme)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInvoice(int id, [FromBody] InvoiceDto dto)
        {
            if (dto == null || id != dto.InvoiceId)
                return BadRequest("Invoice data is invalid or ID mismatch.");

            var invoice = new Invoice
            {
                InvoiceId     = dto.InvoiceId,
                CustomerId    = dto.CustomerId,
                InvoiceNumber = dto.InvoiceNumber,
                InvoiceDate   = dto.InvoiceDate,
                TotalAmount   = dto.TotalAmount,
                UserId        = dto.UserId,
                RecordDate    = dto.RecordDate
            };

            var updated = await _invoiceService.UpdateInvoiceAsync(id, invoice);
            if (!updated)
                return NotFound();

            return Ok(invoice);
        }

        // DELETE: api/invoice/5 (fatura silme)
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInvoice(int id)
        {
            var deleted = await _invoiceService.DeleteInvoiceAsync(id);
            if (!deleted)
                return NotFound();

            return Ok();
        }
    }
}
