using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OrderManagemenSystem.Data.Entities.Table
{
    public class SpOrderDetail
    {
        public long OrderNo {get;set;}
        public int? ID {get;set;}
        public string? CustomerName {get;set;}
        public DateTime? Date {get;set;}
        public string? ItemName {get;set;}
        public string? CateoryName {get;set;}
        public decimal Price {get;set;}
        public byte? Qty {get;set;}
        public decimal? TotalAmount { get; set; }
        public int TotalCount {get;set;}
    }
}
