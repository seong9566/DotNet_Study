using Microsoft.EntityFrameworkCore;
using NetCore.DataBase.Data.DBModels;
using NetCore.DataBase.Data.Repository;
using NetCore_Data.ViewModels;
using NetCore_Services.Interfaces;

namespace NetCore_Services.Svcs;

/// <summary>
/// DataModel: Database와 연동할 Model(Flutter의 dto와 같음)
/// ViewModel : View를 위한 Model
/// </summary>
public class UserService : IUser
{
    // private IUser _userImplementation;
    private readonly WorksContext _context;

    public UserService(WorksContext dbContext)
    {
        _context = dbContext;
    }

    // region : 외부에서 사용 하지 않을 private 함수들은 #region - #endregion 묶어준다.
    #region private methods
    // DB에서 사용자 정보를 가져오는 함수
    // User : 데이터 베이스에서 가져온 값
    private IEnumerable<User> GetUserInfos()

    {
        var users = _context.Users.ToList();

        foreach (var user in users)
        {
            Console.WriteLine($"UserId: {user.UserId}, UserName: {user.UserName}, Email: {user.UserEmail}");
        }

        return users;
    }

    private User GetUserInfo(string userId, string password)
    {
        User? user;
        // FromSql을 사용할땐 컬럼 값이 모두 들어가야 함. userId/password는 파라미터로 전달해 SQL 인젝션을 방지.
        user = _context.Users
            .FromSqlInterpolated($@"SELECT UserId,
                                           UserName,
                                           UserEmail,
                                           Password,
                                           IsMembershipWithdrawn,
                                           AccessFailedCount,
                                           JoinedUtcDate
                                    FROM User
                                    WHERE UserId = {userId} AND Password = {password}")
            .AsNoTracking()
            .FirstOrDefault();

        if (user == null)
        {
            // 로그인 실패 시 실패 횟수 증가 (사용자 기준)
            int rowAffected;
            
            // SQL 문 직접 작성
            // rowAffected = _context.Database.ExecuteSqlInterpolated(
            //     $@"UPDATE User
            //        SET AccessFailedCount = AccessFailedCount + 1
            //        WHERE UserId = {userId}");
            //
            // STORED PROCEDURE
            rowAffected = _context.Database.ExecuteSqlInterpolated($"CALL FailedLoginByUserId({userId})");
        }

        return user;
    }

    private User GetUserInfo(string userId)
    {
        return _context.Users.Where(u => u.UserId.Equals(userId)).FirstOrDefault();
    }

    private IEnumerable<UserRolesByUser> GetUserRolesByUserInfos(string userId)
    {
        var userRolesByUserInfos = _context.UserRolesByUsers.Where(uru => uru.UserId.Equals(userId)).ToList();

        foreach (var role in userRolesByUserInfos)
        {
            role.Role = GetUserRole(role.RoleId);
        }
        return userRolesByUserInfos.OrderByDescending(uru=> uru.Role.RolePriority);
    }

    private UserRole GetUserRole(string roleId)
    {
        return _context.UserRoles.Where(ur => ur.RoleId.Equals(roleId)).FirstOrDefault();
    }


    // ID,PW 체크 함수
    private bool checkUserInfo(string userId, string password)
    {   
        // 1. GetUserInfos로 DB에서 데이터 가져옴.
        // 2. 파람 값으로 받은 userId,passsword를 서버에서 가져온 데이터로 일치 하는지 비교
        // 3. bool값 리턴 
        // return GetUserInfos().Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).Any();

        return GetUserInfo(userId, password) != null ? true : false;
    }
    
    #endregion

    // 인터페이스 구현부 
    bool IUser.MatchTheUserInfo(LoginInfo loginInfo)
    {
        return checkUserInfo(loginInfo.UserName, loginInfo.UserPassword);
    }

    User IUser.GetUserInfo(string userId)
    {
        return GetUserInfo(userId);
    }
    
    IEnumerable<UserRolesByUser> IUser.GetRolesOwnedByUser(string userId)
    {
        return GetUserRolesByUserInfos(userId);
    }
}
