using Microsoft.AspNetCore.Mvc;
using OrderFlowApi.Services;

namespace OrderFlowApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly ProductService _productService;

        public ProductController(ProductService productService)
        {
            _productService = productService;
        }


    }
}
