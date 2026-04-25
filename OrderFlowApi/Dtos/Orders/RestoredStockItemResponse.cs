namespace OrderFlowApi.Dtos.Orders
{
    public class RestoredStockItemResponse
    {
        public int ProductId { get; set; }

        public string ProductName { get; set; } = string.Empty;

        public int RestoredQuantity { get; set; }

        public int CurrentStockQuantity { get; set; }
    }
}
