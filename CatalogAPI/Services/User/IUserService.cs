using CatalogAPI.Models;

namespace CatalogAPI.Services.User
{
    public interface IUserService
    {
        bool IsEmailTaken(string email);     
        List<UserModel> GetDoctorsWithPatients();
        UserModel CreateUser(CreateUserModel createUserModel);        
        Task<bool> ValidateCredentials(string email, string password);
        Task<UserModel> GetUserByEmailAsync(string email);


    }
}
