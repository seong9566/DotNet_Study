using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace NetCore.DataBase.Data.DBModels;

public partial class UserRole
{ 
    [Key]
    public string RoleId { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public sbyte RolePriority { get; set; }

    public DateTime ModifiedUtcDate { get; set; }

    public virtual ICollection<UserRolesByUser> UserRolesByUsers { get; set; } = new List<UserRolesByUser>();
}
