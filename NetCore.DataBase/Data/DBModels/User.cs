using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore.DataBase.Data.DBModels;

public partial class User
{
    [Key]
    public string UserId { get; set; } = null!;

    public string UserName { get; set; } = null!;

    public string UserEmail { get; set; } = null!;

    public string Password { get; set; } = null!;

    public bool IsMembershipWithdrawn { get; set; }

    public DateTime JoinedUtcDate { get; set; }

    public virtual ICollection<UserRolesByUser> UserRolesByUsers { get; set; } = new List<UserRolesByUser>();
}
