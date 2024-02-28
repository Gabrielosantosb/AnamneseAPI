using CatalogAPI.Models;

namespace CatalogAPI.Services.User
{
    public interface IUserService
    {
        bool IsEmailTaken(string email);
        //UserModel CreateUser(UserModel userModel);
        UserModel CreateUser(CreateUserModel createUserModel);
        UserModel ValidateCredentials(string email, string password);

    }
}
