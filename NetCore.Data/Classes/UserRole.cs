using NetCore.Data.Classes;

namespace NetCore_Data.Classes;

public class UserRole
{
    public string RoleId { get; set; } = null!;

    public string RoleName { get; set; } = null!;

    public sbyte RolePriority { get; set; }

    public DateTime ModifiedUtcDate { get; set; }

    public virtual ICollection<UserRolesByUser> UserRolesByUsers { get; set; } = new List<UserRolesByUser>();
}