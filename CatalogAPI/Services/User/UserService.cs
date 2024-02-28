using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace CatalogAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly List<UserModel> users;
        private readonly MySQLContext _dbContext;

        public UserService(MySQLContext dbContext)
        {
            _dbContext = dbContext;
        }

        public bool IsEmailTaken(string email)
        {
            return _dbContext.Users.Any(u => u.Email == email);
        }

        public UserModel CreateUser(CreateUserModel userModel)
        {
            
            if (IsEmailTaken(userModel.Email))
            {
                throw new Exception("E-mail já em uso. Escolha outro.");
            }
            
            UserModel newUser = new UserModel
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                Password = userModel.Password
            };


            _dbContext.Users.Add(newUser);         
            _dbContext.SaveChanges();

            return newUser;
        }

        public UserModel ValidateCredentials(string email, string password)
        {
            
            return _dbContext.Users.FirstOrDefault(u => u.Email == email && u.Password == password);
        }
    }
}
