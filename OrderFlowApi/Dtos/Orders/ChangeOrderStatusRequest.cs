namespace OrderFlowApi.Dtos.Orders
{
    public class ChangeOrderStatusRequest
    {
        public int OrderId { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}