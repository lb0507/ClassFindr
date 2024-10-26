using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IUserData
    {
        Task<bool> SignIn(string username, string password);
        Task CreateUser(UserModel user);
    }
}