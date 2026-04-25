namespace OrderFlowApi.Dtos.Orders
{
    public class CancelOrderResponse
    {
        public int OrderId { get; set; }

        public string Status { get; set; } = string.Empty;

        public List<RestoredStockItemResponse> RestoredItems { get; set; } = new();
    }
}
