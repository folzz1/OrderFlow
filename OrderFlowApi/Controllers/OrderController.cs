using Microsoft.AspNetCore.Mvc;
using OrderFlowApi.Dtos.Orders;
using OrderFlowApi.Services;

namespace OrderFlowApi.Controllers
{
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly OrderService _orderService;

        public OrderController(OrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<ActionResult<OrderResponse>> Create(CreateOrderRequest request)
        {
            var createdOrder = await _orderService.CreateAsync(request);

            return Ok(createdOrder);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OrderDetailsResponse>> GetById(int id)
        {
            var order = await _orderService.GetByIdAsync(id);

            if (order == null)
                return NotFound();

            return Ok(order);
        }

        [HttpPost("{id}/start")]
        public async Task<ActionResult<OrderResponse>> Start(int id)
        {
            var order = await _orderService.StartAsync(id);

            if (order == null)
                return BadRequest();

            return Ok(order);
        }

        [HttpPost("{id}/complete")]
        public async Task<ActionResult<OrderResponse>> Complete(int id)
        {
            var order = await _orderService.CompleteAsync(id);

            if (order == null)
                return BadRequest();

            return Ok(order);
        }

        [HttpPost("{id}/cancel")]
        public async Task<ActionResult<CancelOrderResponse>> Cancel(int id)
        {
            var order = await _orderService.CancelAsync(id);

            if (order == null)
                return BadRequest();

            return Ok(order);
        }
    }
}