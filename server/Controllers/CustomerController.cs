using InvoiceAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace server.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customer (tüm müşterileri alma)
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDTO>>> GetCustomers()
        {
            var customers = await _customerService.GetCustomersAsync();
            var customerDtos = new List<CustomerDTO>();
            foreach (var customer in customers)
            {
                customerDtos.Add(new CustomerDTO
                {
                    CustomerId = customer.CustomerId,
                    TaxNumber = customer.TaxNumber,
                    Title = customer.Title,
                    Address = customer.Address,
                    EMail = customer.EMail,
                    UserId = customer.UserId,
                    RecordDate = customer.RecordDate
                });
            }

            return Ok(customerDtos);
        }


    }
}
