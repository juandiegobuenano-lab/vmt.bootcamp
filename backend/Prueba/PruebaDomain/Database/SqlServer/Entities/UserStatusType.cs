using System;
using System.Collections.Generic;

namespace PruebaDomain.Database.SqlServer.Entities;

public partial class UserStatusType
{
    public int UserStatusTypeId { get; set; }

    public string Code { get; set; } = null!;

    public string ShowName { get; set; } = null!;

    public DateTime CreatedAt { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
