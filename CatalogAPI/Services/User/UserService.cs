using CatalogAPI.Context;
using CatalogAPI.Models;
using Microsoft.EntityFrameworkCore;
using static CatalogAPI.Repository.Repository;

namespace CatalogAPI.Services.User
{
    public class UserService : IUserService
    {
        private readonly List<UserModel> users;
        private readonly MySQLContext _context;
        private readonly BaseRepository<UserModel> _userRepository;

        public UserService(MySQLContext context, BaseRepository<UserModel> userRepository)
        {
            _context = context;
            _userRepository = userRepository;
        }

        public bool IsEmailTaken(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        public UserModel CreateUser(CreateUserModel userModel)
        {
            //Verificar se email já existe
            if (_userRepository.ExistsByEmail(userModel.Email))
            {
                throw new Exception("E-mail já está em uso.");

            }

            //Criptografar senha
            var salt = BCrypt.Net.BCrypt.GenerateSalt(10);
            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(userModel.Password, salt);

            UserModel newUser = new UserModel
            {
                UserName = userModel.UserName,
                Email = userModel.Email,
                Password = hashedPassword
            };

            var createdUser = _userRepository.Create(newUser);
            return createdUser;            
        }


        public async Task<bool> ValidateCredentials(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);

            if (user != null)
            {
                bool isPasswordCorrect = VerifyPassword(password, user.Password);

                return isPasswordCorrect;
            }

            return false;
        }

        private bool VerifyPassword(string enteredPassword, string storedPassword)
        {        
            return BCrypt.Net.BCrypt.Verify(enteredPassword, storedPassword);
        }

        public async Task<UserModel> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public List<UserModel> GetDoctorsWithPatients()
        {
            return _context.Users.Include(u => u.Patients).Where(u => u.Patients.Any()).ToList();
        }
    }
}
