using System;
using System.Collections.Generic;

namespace OrderManagemenSystem.Data.Entities.Table;

public partial class Order
{
    public int Id { get; set; }

    public DateTime? Date { get; set; }

    public string? CustomerName { get; set; }

    public byte? Qty { get; set; }

    public int ItemId { get; set; }

    public virtual Item Item { get; set; } = null!;
}
