using Microsoft.EntityFrameworkCore;
using OrderFlowApi.Data;
using OrderFlowApi.Dtos.OrderItems;
using OrderFlowApi.Models;

namespace OrderFlowApi.Services
{
    public class OrderItemService
    {
        private readonly AppDbContext _dbContext;

        public OrderItemService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderItemResponse?> AddAsync(AddOrderItemRequest request)
        {
            var order = await _dbContext.Orders.FindAsync(request.OrderId);

            if (order == null || order.Status != "Created")
                return null;

            var product = await _dbContext.Products.FindAsync(request.ProductId);

            if (product == null)
                return null;

            if (request.Quantity <= 0)
                return null;

            var orderItem = new OrderItem
            {
                OrderId = order.Id,
                ProductId = product.Id,
                Quantity = request.Quantity,
                PriceAtPurchase = product.Price
            };

            _dbContext.OrderItems.Add(orderItem);
            await _dbContext.SaveChangesAsync();

            return new OrderItemResponse
            {
                Id = orderItem.Id,
                OrderId = orderItem.OrderId,
                ProductId = orderItem.ProductId,
                ProductName = product.Name,
                Quantity = orderItem.Quantity,
                PriceAtPurchase = orderItem.PriceAtPurchase,
                TotalPrice = orderItem.PriceAtPurchase * orderItem.Quantity
            };
        }

        public async Task<OrderItemsResponse?> GetByOrderIdAsync(int orderId)
        {
            var orderExists = await _dbContext.Orders.AnyAsync(o => o.Id == orderId);

            if (!orderExists)
                return null;

            var items = await _dbContext.OrderItems
                .Where(item => item.OrderId == orderId)
                .Include(item => item.Product)
                .Select(item => new OrderItemResponse
                {
                    Id = item.Id,
                    OrderId = item.OrderId,
                    ProductId = item.ProductId,
                    ProductName = item.Product != null ? item.Product.Name : string.Empty,
                    Quantity = item.Quantity,
                    PriceAtPurchase = item.PriceAtPurchase,
                    TotalPrice = item.PriceAtPurchase * item.Quantity
                })
                .ToListAsync();

            return new OrderItemsResponse
            {
                OrderId = orderId,
                Items = items
            };
        }

        public async Task<bool> DeleteAsync(int orderId, int itemId)
        {
            var order = await _dbContext.Orders.FindAsync(orderId);

            if (order == null || order.Status != "Created")
                return false;

            var item = await _dbContext.OrderItems
                .FirstOrDefaultAsync(i => i.Id == itemId && i.OrderId == orderId);

            if (item == null)
                return false;

            _dbContext.OrderItems.Remove(item);
            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}