using System;
using System.Collections.Generic;

namespace PruebaDomain.Database.SqlServer.Entities;

public partial class Item
{
    public int ItemId { get; set; }

    public string Name { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Collection> Collections { get; set; } = new List<Collection>();
}
