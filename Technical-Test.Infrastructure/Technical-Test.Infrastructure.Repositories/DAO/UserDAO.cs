using System.Diagnostics;

namespace Technical_Test.Infrastructure.Repositories.DAO
{
    [DebuggerDisplay("UserId = {Id} Name = {Name}")]
    public class UserDAO
    {
        public Guid Id { get; set; }
        public Guid ProviderId { get; set; }
        public string Name { get; set; } = string.Empty;
    }
}
