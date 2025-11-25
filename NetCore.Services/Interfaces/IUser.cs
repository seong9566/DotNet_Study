using NetCore_Data.ViewModels;

namespace NetCore_Services.Interfaces;

public interface IUser
{
    bool MatchTheUserInfo(LoginInfo loginInfo);
}