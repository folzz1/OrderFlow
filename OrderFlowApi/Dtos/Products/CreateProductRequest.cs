namespace OrderFlowApi.Dtos.Products
{
    public class CreateProductRequest
    {
        public string Name { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }
    }
}