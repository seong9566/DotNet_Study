using NetCore_Data.DataModels;
using NetCore_Data.ViewModels;
using NetCore_Services.Interfaces;

namespace NetCore_Services.Svcs;

/// <summary>
/// DataModel: Database와 연동할 Model(Flutter의 dto와 같음)
/// ViewModel : View를 위한 Model
/// </summary>
public class UserService : IUser
{
    private IUser _userImplementation;

    // region : 외부에서 사용 하지 않을 private 함수들은 #region - #endregion 묶어준다.
    #region private methods
    // DB에서 사용자 정보를 가져오는 함수
    // User : 데이터 베이스에서 가져온 값
    private IEnumerable<User> GetUserInfos()
    {
        return new List<User>()
        {
            new User
            {
                UserId = "testId",
                UserName = "테스트",
                UserEmail = "test@gmail.com",
                Password = "1234",
            }
        };
    }


    // ID,PW 체크 함수
    private bool checkUserInfo(string userId, string password)
    {   
        // 1. GetUserInfos로 DB에서 데이터 가져옴.
        // 2. 파람 값으로 받은 userId,passsword를 서버에서 가져온 데이터로 일치 하는지 비교
        // 3. bool값 리턴 
        return GetUserInfos().Where(u => u.UserId.Equals(userId) && u.Password.Equals(password)).Any();
    }
    
    #endregion

    // 인터페이스 구현부 
    bool IUser.MatchTheUserInfo(LoginInfo loginInfo)
    {
        return checkUserInfo(loginInfo.UserName, loginInfo.UserPassword);
    }
}