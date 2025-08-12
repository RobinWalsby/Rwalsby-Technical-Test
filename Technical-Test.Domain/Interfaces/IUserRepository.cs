using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;

namespace Technical_Test.Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        User Get(UserId id);
        void Add(User user);
        void Delete(UserId id);
    }
}
