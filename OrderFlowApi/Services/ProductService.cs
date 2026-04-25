using Microsoft.EntityFrameworkCore;
using OrderFlowApi.Data;
using OrderFlowApi.Dtos.Products;
using OrderFlowApi.Models;

namespace OrderFlowApi.Services
{
    public class ProductService
    {
        private readonly AppDbContext _dbContext;

        public ProductService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<ProductResponse> CreateAsync(CreateProductRequest request)
        {
            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                StockQuantity = request.StockQuantity
            };

            _dbContext.Products.Add(product);
            await _dbContext.SaveChangesAsync();

            return MapToResponse(product);
        }

        public async Task<List<ProductResponse>> GetAllAsync()
        {
            return await _dbContext.Products
                .Select(product => new ProductResponse
                {
                    Id = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    StockQuantity = product.StockQuantity
                })
                .ToListAsync();
        }

        public async Task<ProductResponse?> GetByIdAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
                return null;

            return MapToResponse(product);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var product = await _dbContext.Products.FindAsync(id);

            if (product == null)
                return false;

            _dbContext.Products.Remove(product);
            await _dbContext.SaveChangesAsync();

            return true;
        }

        private static ProductResponse MapToResponse(Product product)
        {
            return new ProductResponse
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                StockQuantity = product.StockQuantity
            };
        }
    }
}