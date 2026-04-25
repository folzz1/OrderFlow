namespace OrderFlowApi.Dtos.OrderItems
{
    public class AddOrderItemRequest
    {
        public int OrderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }
    }
}