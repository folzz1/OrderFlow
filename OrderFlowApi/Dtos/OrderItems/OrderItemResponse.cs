namespace OrderFlowApi.Dtos.OrderItems
{
    public class OrderItemResponse
    {
        public int Id { get; set; }

        public int OrderId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}