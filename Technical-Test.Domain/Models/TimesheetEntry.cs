using System.Diagnostics;
using Technical_Test.Domain.ValueObjects;

namespace Technical_Test.Domain.Models
{
    [DebuggerDisplay("UserId = {UserId.Value} Date = {Date.Value} Hours = {Hours.Value}")]
    public class TimesheetEntry
    {
        public TimesheetId Id { get; private set; } = TimesheetId.Empty;

        public UserId UserId { get; set; } = UserId.Empty;

        public ProjectId ProjectId { get; set; } = ProjectId.Empty;

        public TimesheetDate Date { get; private set; } = TimesheetDate.Empty;

        public TimesheetHours Hours { get; set; } = TimesheetHours.Empty;

        public TimesheetDescription Description { get; set; } = TimesheetDescription.Empty;

        public static TimesheetEntry Create(
                TimesheetId id,
                UserId userId,
                ProjectId projectId,
                TimesheetDate date,
                TimesheetHours hours,
                TimesheetDescription description)
        {
            return new TimesheetEntry() { Id = id, UserId = userId, ProjectId = projectId, Date = date, Hours = hours, Description = description };
        }
    }
}
