using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetCore_Data.DataModels;

public class UserRole
{
    [Key,StringLength(50),Column(TypeName = "varchar(50)")]
    public string RoleId { get; set; }

    [Required,StringLength(100),Column(TypeName = "nvarchar(100)")]
    public string RoleName { get; set; }

    [Required]
    public byte RolePriority { get; set; }

    [Required]
    public DateTime ModifiedUtcDate { get; set; }

    // FK 지정 
    [ForeignKey("RoleId")]
    public virtual ICollection<UserRoleByUser> UserRolesByUsers { get; set; }
}