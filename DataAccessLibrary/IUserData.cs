using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IUserData
    {
        Task<Tuple<bool, string>> SignIn(string username, string password);
        public void SignOut();
        Task CreateUser(UserModel user);
        public UserModel? GetUserSignOnInfo();

    }
}