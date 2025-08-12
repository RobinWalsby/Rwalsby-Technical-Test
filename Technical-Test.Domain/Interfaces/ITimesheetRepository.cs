using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;

namespace Technical_Test.Domain.Interfaces.Repositories
{
    public interface ITimesheetRepository
    {
        void Add(TimesheetEntry entry);

        void Update(TimesheetEntry updateEntry);

        TimesheetEntry? GetEntry(UserId id, ProjectId projectId, TimesheetDate date);

        List<TimesheetEntry> GetEntries(UserId id);

        List<TimesheetEntry> GetEntries(UserId id, TimesheetDate date);

        void DeleteAll(UserId id);

        void Delete(TimesheetId id);
    }
}
