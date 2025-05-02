using InvoiceAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace InvoiceAPI.Services
{
    public class CustomerService
    {
        private readonly DataContext _context;

        public CustomerService(DataContext context)
        {
            _context = context;
        }

        // Tüm müsterileri alma
        public async Task<IEnumerable<Customer>> GetCustomersAsync()
        {
            return await _context.Customers.ToListAsync();
        }
    }
    
}
