using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderManagemenSystem.Business.Contracts;
using OrderManagemenSystem.Common.DTOs;

namespace OrderManagemenSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost("GetOrderDetails")]
        public async Task<IActionResult> GetOrderDetails(GridSearchRequest request)
        {
            return Ok(await _orderService.GetOrderDetails(request));
        }

        [HttpGet("GetAllCategories")]
        public async Task<IActionResult> GetAllCategories()
        {
            var result = await _orderService.GetAllCategories();
            return Ok(result);
        }

        [HttpPost("InsertOrder")]
        public async Task<IActionResult> InsertOrder(OrderRequest orderRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.SaveOrder(orderRequest);
                if (result > 0)
                {
                    return Ok("Order Saved Sucessfully");
                }
                return StatusCode(500, "Something went wrong!");
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("DeleteOrder")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            if (id > 0)
            {
                var result = await _orderService.DeleteOrder(id);
                if (result == 0)
                {
                    return NotFound("Order not found");
                }
                else if (result > 0)
                {
                    return Ok("Order deleted successfully@");
                }
                return StatusCode(500, "Something went wrong!");
            }
            return BadRequest("Please select order");
        }

        [HttpGet("GetOrder")]
        public async Task<IActionResult> Get(int id)
        {
            if (id > 0)
            { 
                var result = await _orderService.GetVwOrderDetail(id);
                if(result!=null)
                {
                    return Ok(result);
                }
                return NotFound("Order not found");
            }
            return BadRequest("Please select order");
        }

        [HttpPut("UpdateOrder")]
        public async Task<IActionResult> UpdateOrder(OrderUpdateRequest updateRequest)
        {
            if (ModelState.IsValid)
            {
                var result = await _orderService.UpdateOrder(updateRequest);
                if (result == 0)
                {
                    return NotFound("Order not found");
                }
                else if (result > 0)
                {
                    return Ok("Order updated successfully@");
                }
                return StatusCode(500, "Something went wrong!");
            }
            return BadRequest(ModelState);
        }


    }
}
