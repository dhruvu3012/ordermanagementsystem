using System.ComponentModel.DataAnnotations;

namespace OrderManagemenSystem.Common.DTOs
{

    public class OrderRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="Please enter customer name")]
        [StringLength(50,MinimumLength = 2, ErrorMessage = "Customer name must be in 3 to 50 characters")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Please enter date")]
        public DateTime? OrderDate { get; set; }
        [Required, MinLength(1,ErrorMessage = "Order items are required")]
        public List<OrderListRequest> Orders { get; set; }
    }

    public class OrderListRequest
    {
        [Range(1,int.MaxValue,ErrorMessage = "Please select item")]
        public int ItemID { get; set; }
        [Range(1, int.MaxValue, ErrorMessage = "Please select category")]
        public int CategoryId { get; set; }
        [Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100.")]
        public byte Qty { get; set; }
    }

    public class OrderUpdateRequest : OrderListRequest
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "Please enter customer name")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Customer name must be in 3 to 50 characters")]
        public string? CustomerName { get; set; }
        [Required(ErrorMessage = "Please enter date")]
        public DateTime OrderDate { get; set; }
    }
}
