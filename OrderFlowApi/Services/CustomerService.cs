using Microsoft.EntityFrameworkCore;
using OrderFlowApi.Data;
using OrderFlowApi.Dtos.Customers;
using OrderFlowApi.Models;

namespace OrderFlowApi.Services
{
    public class CustomerService
    {
        private readonly AppDbContext _dbContext;

        public CustomerService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<CustomerResponse> CreateAsync(CreateCustomerRequest request)
        {
            var customer = new Customer
            {
                Name = request.Name,
                Email = request.Email
            };

            _dbContext.Customers.Add(customer);
            await _dbContext.SaveChangesAsync();

            return MapToResponse(customer);
        }

        public async Task<List<CustomerResponse>> GetAllAsync()
        {
            return await _dbContext.Customers
                .Select(customer => new CustomerResponse
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Email = customer.Email
                })
                .ToListAsync();
        }

        public async Task<CustomerResponse?> GetByIdAsync(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
                return null;

            return MapToResponse(customer);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _dbContext.Customers.FindAsync(id);

            if (customer == null)
                return false;

            _dbContext.Customers.Remove(customer);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private static CustomerResponse MapToResponse(Customer customer)
        {
            return new CustomerResponse
            {
                Id = customer.Id,
                Name = customer.Name,
                Email = customer.Email
            };
        }
    }
}