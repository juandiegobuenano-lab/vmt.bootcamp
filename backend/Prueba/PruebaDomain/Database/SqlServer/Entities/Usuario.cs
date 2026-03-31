using System;
using System.Collections.Generic;

namespace PruebaDomain.Database.SqlServer.Entities;

public partial class Usuario
{
    public Guid UserId { get; set; }

    public string Username { get; set; } = null!;

    public string DisplayName { get; set; } = null!;

    public string? Description { get; set; }

    public int StatusType { get; set; }

    public string? StatusContent { get; set; }

    public string? AvatarUrl { get; set; }

    public string? BannerUrl { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual UserStatusType StatusTypeNavigation { get; set; } = null!;

    public virtual ICollection<Role> Roles { get; set; } = new List<Role>();
}
