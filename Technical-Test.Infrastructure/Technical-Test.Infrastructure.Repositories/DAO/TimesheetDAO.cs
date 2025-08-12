using System.Diagnostics;

namespace Technical_Test.Infrastructure.Repositories.DAO
{
    [DebuggerDisplay("UserId = {UserId} Date = {Date}")]
    public class TimesheetDAO
    {
        public Guid Id { get; set; }
        public Guid UserId { get; set; }
        public Guid ProjectId { get; set; }
        public DateOnly Date { get; set; } = DateOnly.MinValue;
        public decimal Hours { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
        public DateTime CreatedOn { get; set; } = DateTime.MinValue;
    }
}
