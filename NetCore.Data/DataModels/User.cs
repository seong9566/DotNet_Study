// using System.ComponentModel.DataAnnotations;
// using System.ComponentModel.DataAnnotations.Schema;
//
// namespace NetCore_Data.DataModels;
//
// /// 1. 데이터 어노테이션 작성
//
// /// <summary>
// /// Key : Primary Key로 지정 
// /// StringLength : 문자열의 최대 길이 설정
// /// Column(TypeName = "varchar(50)") : 컬럼의 타입 강제 지정
// /// Required : 필수 값
// /// </summary>
// public class User
// {
//     [Key,StringLength(50),Column(TypeName = "varchar(50)")]
//     public string UserId { get; set; }
//     [Required,StringLength(100),Column(TypeName = "nvarchar(100)")]
//     public string UserName { get; set; }
//     
//     [Required,StringLength(320),Column(TypeName = "varchar(320)")]
//     public string UserEmail { get; set; }
//     
//     [Required,StringLength(130),Column(TypeName = "varchar(130)")]
//     public string Password { get; set; }
//
//     [Required]
//     public bool IsMembershipWithDrawn { get; set; }
//
//     [Required]
//     public DateTime JoinedUtcDate { get; set; }
//     
//     // FK 지정 
//     // UserId로 FK를 지정 
//     [ForeignKey("UserId")]
//     public virtual ICollection<UserRoleByUser> UserRolesByUser { get; set; }
// }