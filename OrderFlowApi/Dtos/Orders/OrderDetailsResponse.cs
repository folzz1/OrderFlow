using OrderFlowApi.Dtos.OrderItems;

namespace OrderFlowApi.Dtos.Orders
{
    public class OrderDetailsResponse
    {
        public int Id { get; set; }

        public int CustomerId { get; set; }

        public DateTime CreatedAt { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<OrderItemResponse> Items { get; set; } = new();

        public decimal TotalPrice { get; set; }
    }
}