using System;
using System.Collections.Generic;

namespace PruebaDomain.Database.SqlServer.Entities;

public partial class Role
{
    public int RoleId { get; set; }

    public string Code { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Usuario> Users { get; set; } = new List<Usuario>();
}
