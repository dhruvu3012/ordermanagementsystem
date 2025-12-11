using System;
using System.Collections.Generic;

namespace OrderManagemenSystem.Data.Entities.Table;

public partial class Item
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public string? Name { get; set; }

    public decimal? Price { get; set; }

    public virtual Category? Category { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();
}
