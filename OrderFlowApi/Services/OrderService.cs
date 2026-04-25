using Microsoft.EntityFrameworkCore;
using OrderFlowApi.Data;
using OrderFlowApi.Dtos.OrderItems;
using OrderFlowApi.Dtos.Orders;
using OrderFlowApi.Models;

namespace OrderFlowApi.Services
{
    public class OrderService
    {
        private readonly AppDbContext _dbContext;

        public OrderService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<OrderResponse> CreateAsync(CreateOrderRequest request)
        {
            var order = new Order
            {
                CustomerId = request.CustomerId
            };

            _dbContext.Orders.Add(order);
            await _dbContext.SaveChangesAsync();

            return MapToResponse(order);
        }

        public async Task<OrderDetailsResponse?> GetByIdAsync(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null;

            var totalPrice = order.Items.Sum(i => i.PriceAtPurchase * i.Quantity);

            return new OrderDetailsResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
                TotalPrice = totalPrice,

                Items = order.Items.Select(i => new OrderItemResponse
                {
                    Id = i.Id,
                    OrderId = i.OrderId,
                    ProductId = i.ProductId,
                    ProductName = i.Product?.Name ?? "",
                    Quantity = i.Quantity,
                    PriceAtPurchase = i.PriceAtPurchase,
                    TotalPrice = i.PriceAtPurchase * i.Quantity
                }).ToList()
            };
        }

        public async Task<OrderResponse?> StartAsync(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null || order.Status != "Created")
                return null;

            if (!order.Items.Any())
                return null;

            foreach (var item in order.Items)
            {
                if (item.Product == null)
                    return null;

                if (item.Product.StockQuantity < item.Quantity)
                    return null;
            }

            foreach (var item in order.Items)
            {
                item.Product!.StockQuantity -= item.Quantity;
            }

            order.Status = "InProgress";

            await _dbContext.SaveChangesAsync();

            return MapToResponse(order);
        }

        public async Task<OrderResponse?> CompleteAsync(int id)
        {
            var order = await _dbContext.Orders.FindAsync(id);

            if (order == null || order.Status != "InProgress")
                return null;

            order.Status = "Completed";

            await _dbContext.SaveChangesAsync();

            return MapToResponse(order);
        }

        public async Task<CancelOrderResponse?> CancelAsync(int id)
        {
            var order = await _dbContext.Orders
                .Include(o => o.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
                return null;

            if (order.Status == "Completed" || order.Status == "Cancelled")
                return null;

            var restoredItems = new List<RestoredStockItemResponse>();

            if (order.Status == "InProgress")
            {
                foreach (var item in order.Items)
                {
                    if (item.Product == null)
                        continue;

                    item.Product.StockQuantity += item.Quantity;

                    restoredItems.Add(new RestoredStockItemResponse
                    {
                        ProductId = item.ProductId,
                        ProductName = item.Product.Name,
                        RestoredQuantity = item.Quantity,
                        CurrentStockQuantity = item.Product.StockQuantity
                    });
                }
            }

            order.Status = "Cancelled";

            await _dbContext.SaveChangesAsync();

            return new CancelOrderResponse
            {
                OrderId = order.Id,
                Status = order.Status,
                RestoredItems = restoredItems
            };
        }

        private static OrderResponse MapToResponse(Order order)
        {
            return new OrderResponse
            {
                Id = order.Id,
                CustomerId = order.CustomerId,
                CreatedAt = order.CreatedAt,
                Status = order.Status,
            };
        }
    }
}
