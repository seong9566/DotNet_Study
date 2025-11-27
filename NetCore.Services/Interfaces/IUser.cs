using NetCore_Data.ViewModels;
using NetCore.DataBase.Data.DBModels;

namespace NetCore_Services.Interfaces;

public interface IUser
{
    bool MatchTheUserInfo(LoginInfo loginInfo);
    User GetUserInfo(string userId);
    IEnumerable<UserRolesByUser> GetRolesOwnedByUser(string userId);
}