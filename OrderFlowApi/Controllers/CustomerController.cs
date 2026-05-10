using Microsoft.AspNetCore.Mvc;
using OrderFlowApi.Dtos.Customers;
using OrderFlowApi.Services;

namespace OrderFlowApi.Controllers
{
    [ApiController]
    [Route("api/customers")]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;

        public CustomerController(CustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost]
        public async Task<ActionResult<CustomerResponse>> Create(CreateCustomerRequest request)
        {
            var createdCustomer = await _customerService.CreateAsync(request);

            return Ok(createdCustomer);
        }

        [HttpGet]
        public async Task<ActionResult<List<CustomerResponse>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();

            return Ok(customers);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CustomerResponse>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);

            if (customer == null)
                return NotFound();

            return Ok(customer);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteById(int id)
        {
            var deleted = await _customerService.DeleteAsync(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }
    }
}