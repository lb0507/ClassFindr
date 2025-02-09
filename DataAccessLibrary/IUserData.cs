﻿using ClassFindrDataAccessLibrary.Models;

namespace ClassFindrDataAccessLibrary
{
    public interface IUserData
    {
        Task<Tuple<bool, string>> SignIn(string username, string password);
        
        public void SignOut();
        
        Task<bool> CreateUser(UserModel user);

        Task<bool> DeleteUser(UserModel user);
        
        UserModel? GetUserSignOnInfo();

        Task<Tuple<bool, string>> ChangePassword(string email, string username, string newPW);
    }
}