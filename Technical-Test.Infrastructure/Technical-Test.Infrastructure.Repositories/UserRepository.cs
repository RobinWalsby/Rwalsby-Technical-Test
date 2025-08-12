using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;
using Technical_Test.Infrastructure.Repositories.DAO;

namespace Technical_Test.Infrastructure.Repositories
{
    public class UserRepository() : IUserRepository
    {
        private readonly List<UserDAO> _users = new List<UserDAO>();

        private User? Map(UserDAO? dao)
        {
            return dao == null ? null : User.Create(
                UserId.Create(dao.Id),
                UserName.Create(dao.Name));
        }

        private UserDAO? Map(User? user)
        {
            return user == null ? null : new UserDAO
            {
                Id = user.UserId.Value,
                Name = user.UserName.Value
            };
        }

        public User Get(UserId id)
        {
            return Map(_users.FirstOrDefault(user => user.Id == id.Value)) 
                        ?? throw new KeyNotFoundException($"User not found with ID {id.Value}.");
        }

        public void Add(User user)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == user.UserId.Value);
            if (existingUser is not null)
            {
                throw new InvalidOperationException($"User already exists with ID {user.UserId.Value}.");
            }
            var userDAO = Map(user);
            if (userDAO != null)
            {
                _users.Add(userDAO);
            }
        }

        public void Delete(UserId id)
        {
            var existingUser = _users.FirstOrDefault(u => u.Id == id.Value);
            if (existingUser is null)
            {
                throw new InvalidOperationException($"User not found with ID {id.Value}.");
            }

            _users.Remove(existingUser);
        }
    }
}
