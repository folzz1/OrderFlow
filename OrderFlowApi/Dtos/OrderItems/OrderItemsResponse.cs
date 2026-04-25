namespace OrderFlowApi.Dtos.OrderItems
{
    public class OrderItemsResponse
    {
        public int OrderId { get; set; }

        public List<OrderItemResponse> Items { get; set; } = new();
    }
}