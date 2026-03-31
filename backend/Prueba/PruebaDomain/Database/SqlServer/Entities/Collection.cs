using System;
using System.Collections.Generic;

namespace PruebaDomain.Database.SqlServer.Entities;

public partial class Collection
{
    public Guid CollectionId { get; set; }

    public string Name { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public DateTime? DeleteAt { get; set; }

    public virtual ICollection<Item> Items { get; set; } = new List<Item>();
}
