using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InvoiceAPI.Services
{
    public class InvoiceService
    {
        private readonly DataContext _context;

        public InvoiceService(DataContext context)
        {
            _context = context;
        }

        // tüm faturaları alma
        public async Task<(List<Invoice> Invoices, int TotalCount)> GetInvoicesAsync(
            DateTime? startDate,
            DateTime? endDate,
            string searchQuery,
            string sort,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Invoices
                .Include(i => i.Customer)
                .AsQueryable();

            // Tarih filtreleme
            if (startDate.HasValue)
                query = query.Where(i => i.InvoiceDate >= startDate.Value);

            if (endDate.HasValue)
                query = query.Where(i => i.InvoiceDate <= endDate.Value);

            // Arama filtreleme
            if (!string.IsNullOrWhiteSpace(searchQuery))
            {
                var loweredQuery = searchQuery.Trim().ToLower();
                query = query.Where(i =>
                    i.InvoiceNumber.ToLower().Contains(loweredQuery) ||
                    i.Customer.Title.ToLower().Contains(loweredQuery));
            }

            // Sıralama
            query = sort switch
            {
                "date_asc" => query.OrderBy(i => i.InvoiceDate),
                "date_desc" => query.OrderByDescending(i => i.InvoiceDate),
                "amount_asc" => query.OrderBy(i => i.TotalAmount),
                "amount_desc" => query.OrderByDescending(i => i.TotalAmount),
                _ => query.OrderByDescending(i => i.InvoiceDate) 
            };

            var totalCount = await query.CountAsync();

            var invoices = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (invoices, totalCount);
        }

        // yeni fatura ekleme
        public async Task<Invoice> AddInvoiceAsync(Invoice invoice)
        {
            _context.Invoices.Add(invoice);
            await _context.SaveChangesAsync();
            return invoice;
        }

        // fatura güncelleme
        public async Task<bool> UpdateInvoiceAsync(int id, Invoice invoice)
        {
            if (id != invoice.InvoiceId)
            {
                return false; // ID mismatch
            }

            _context.Entry(invoice).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!InvoiceExists(id))
                {
                    return false; 
                }
                else
                {
                    throw;
                }
            }
        }

        // fatura silme
        public async Task<bool> DeleteInvoiceAsync(int id)
        {
            var invoice = await _context.Invoices.FindAsync(id);
            if (invoice == null)
            {
                return false; 
            }

            _context.Invoices.Remove(invoice);
            await _context.SaveChangesAsync();
            return true;
        }

        // fatura olup olmadığını kontrol etme
        private bool InvoiceExists(int id)
        {
            return _context.Invoices.Any(e => e.InvoiceId == id);
        }
    }
}
