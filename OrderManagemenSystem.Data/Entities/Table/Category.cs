using System;
using System.Collections.Generic;

namespace OrderManagemenSystem.Data.Entities.Table;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
