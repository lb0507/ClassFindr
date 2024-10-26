using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IUserData
    {
        Task<Tuple<bool, string>> SignIn(string username, string password);
        Task CreateUser(UserModel user);
    }
}