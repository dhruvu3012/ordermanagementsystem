using System;
using System.Collections.Generic;

namespace OrderManagemenSystem.Data.Entities.Table;

public partial class VwOrderDetail
{
    public int Id { get; set; }

    public string? CustomerName { get; set; }

    public DateTime? Date { get; set; }

    public int ItemId { get; set; }

    public string? ItemName { get; set; }

    public int CategoryId { get; set; }

    public string? CateoryName { get; set; }

    public decimal? Price { get; set; }

    public byte? Qty { get; set; }

    public decimal? TotalAmount { get; set; }
}
