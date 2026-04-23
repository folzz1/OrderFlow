namespace OrderFlowApi.Dtos.Orders
{
    public class OrderStatusResponse
    {
        public int OrderId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}