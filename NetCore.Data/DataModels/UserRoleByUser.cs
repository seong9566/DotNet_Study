// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
//
// namespace NetCore_Data.DataModels;
//
// public class UserRoleByUser
// {
//     [Key,StringLength(50),Column(TypeName = "varchar(50)")]
//     public string UserId { get; set; }
//     
//     // 권한도 Primary이기 때문에 복합키가 됌.
//     [Key,StringLength(50),Column(TypeName = "varchar(50)")]
//     public string RoleId  { get; set; }
//     
//     [Required]
//     public DateTime OwnedUtcDate { get; set; }
//     
//     
//     public virtual User User { get; set; }
//     
//     public virtual UserRole UserRole { get; set; }
// }