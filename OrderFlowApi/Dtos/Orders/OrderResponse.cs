using OrderFlowApi.Dtos.OrderItems;

namespace OrderFlowApi.Dtos.Orders
{
    public class OrderResponse
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}