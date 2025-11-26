using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetCore_Data.Classes;

namespace NetCore.Data.Classes;

public partial class UserRolesByUser
{
    public string UserId { get; set; } = null!;

    public string RoleId { get; set; } = null!;

    public DateTime OwnedUtcDate { get; set; }

    public DateTime ModifiedUtcDate { get; set; }

    public virtual UserRole Role { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
