using Microsoft.AspNetCore.Mvc;
using SQRBackend.Models.Dtos;
using SQRBackend.Services;
using System.Threading.Tasks;

namespace SQRBackend.Controllers
{
    [Route("api/orders")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _orderService.GetOrdersAsync();
            return Ok(new { orders });
        }

        [HttpGet("GetProduction")]
        public async Task<IActionResult> GetProduction([FromQuery] string email)
        {
            var productions = await _orderService.GetProductionAsync(email);
            return Ok(new { productions });
        }

        [HttpPost("SetProduction")]
        public async Task<IActionResult> SetProduction([FromBody] SetProductionDto dto)
        {
            var response = await _orderService.SetProductionAsync(dto);
            return StatusCode(response.Status, response);
        }
    }
}