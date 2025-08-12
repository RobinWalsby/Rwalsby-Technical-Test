using System.Diagnostics;
using Technical_Test.Domain.ValueObjects;

namespace Technical_Test.Domain.Models
{
    [DebuggerDisplay("UserId = {UserId.Value} UserName = {UserName.Value}")]
    public class User
    {
        public UserId UserId { get; private set; } = UserId.Empty;

        public UserName UserName { get; private set; } = UserName.Empty;

        public static User Create(
                UserId userId,
                UserName userName)
        {
            return new User() { UserId = userId, UserName = userName };
        }
    }
}
