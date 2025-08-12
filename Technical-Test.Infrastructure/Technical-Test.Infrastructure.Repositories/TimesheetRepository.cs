using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;
using Technical_Test.Infrastructure.Repositories.DAO;

namespace Technical_Test.Infrastructure.Repositories
{
    public class TimesheetRepository() : ITimesheetRepository
    {
        private readonly List<TimesheetDAO> _timesheetEntries = new List<TimesheetDAO>();

        private TimesheetEntry? Map(TimesheetDAO? dao)
        {
            return dao == null ? null : TimesheetEntry.Create(
                TimesheetId.Create(dao.Id),
                UserId.Create(dao.UserId),
                ProjectId.Create(dao.ProjectId),
                TimesheetDate.Create(dao.Date),
                TimesheetHours.Create(dao.Hours),
                TimesheetDescription.Create(dao.Description));
        }

        private TimesheetDAO? Map(TimesheetEntry? entry)
        {
            return entry == null ? null : new TimesheetDAO
            {
                Id = entry.Id.Value,
                UserId = entry.UserId.Value,
                ProjectId = entry.ProjectId.Value,
                Date = entry.Date.Value,
                Hours = entry.Hours.Value!.Value,
                Description = entry.Description.Value
            };
        }

        public void Add(TimesheetEntry entry)
        {
            var entryDAO = Map(entry);
            if (entryDAO != null)
            {
                _timesheetEntries.Add(entryDAO);
            }
        }

        public void Update(TimesheetEntry updateEntry)
        {
            var result = _timesheetEntries
                .FirstOrDefault(entry => entry.Id == updateEntry.Id.Value);

            if (result is null)
            { 
                throw new InvalidOperationException("Entry not found for update.");
            }

            result.Hours = updateEntry.Hours.Value!.Value;
            result.Description = updateEntry.Description.Value;
        }

        public TimesheetEntry? GetEntry(UserId id, ProjectId projectId, TimesheetDate date)
        {
            var result = _timesheetEntries
                .FirstOrDefault(entry => entry.UserId == id.Value && entry.ProjectId == projectId.Value && entry.Date == date.Value);

            return result is null ? null : Map(result);
        }

        public List<TimesheetEntry> GetEntries(UserId id)
        {
            return _timesheetEntries
                .Where(entry => entry.UserId == id.Value)
                .OrderByDescending(dt => dt.Date)
                .Select(Map)
                .Where(entry => entry != null)
                .Cast<TimesheetEntry>()
                .ToList();
        }

        public List<TimesheetEntry> GetEntries(UserId id, TimesheetDate date)
        {
            return _timesheetEntries
                .Where(entry => entry.UserId == id.Value && entry.Date == date.Value)
                .OrderByDescending(dt => dt.Date)
                .Select(Map)
                .Where(entry => entry != null)
                .Cast<TimesheetEntry>()
                .ToList();
        }

        public void DeleteAll(UserId id)
        {
            _timesheetEntries.RemoveAll(entry => entry.UserId == id.Value);
        }

        public void Delete(TimesheetId id)
        {
            _timesheetEntries.RemoveAll(entry => entry.Id == id.Value);
        }
    }
}
