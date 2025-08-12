using MediatR;
using System.Diagnostics;
using Technical_Test.Application.Base;

namespace TechnicalTest.Application.Timesheets
{
    public class TimesheetsRequestQuery : IRequest<TimesheetsRequestResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string UserNameSearch { get; set; } = string.Empty;

        public string WeekNumber { get; set; } = string.Empty;

        public TimesheetsRequestQuery(string userId)
        {
            this.UserId = userId;
        }
    }

    [DebuggerDisplay("TimeSheets Count = {DailyTimesheets.Count()}")]
    public class TimesheetsRequestResponse : BaseResponse
    {
        public List<TimesheetDailyResponse> DailyTimesheets { get; set; } = new List<TimesheetDailyResponse>();

        public List<TimesheetProjectResponse> Projects { get; set; } = new List<TimesheetProjectResponse>();
    }

    [DebuggerDisplay("Date = {Date.ToString()} Hours={HoursDisplay} Description={Description}")]
    public class TimesheetDailyResponse
    {
        public string Id { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string ProjectName { get; set; } = string.Empty;
        public string ProjectDisplay { get => ProjectName == string.Empty ? $"<Name is unset> ({ProjectId})" : ProjectName; }
        public string Date { get; set; } = string.Empty;
        public string HoursDisplay { get; set; } = string.Empty;
        public decimal Hours { get; set; } = 0;
        public int WeekOfYear { get; set; } = 0;
        public string Description { get; set; } = string.Empty;
    }

    [DebuggerDisplay("ProjectId = {ProjectId.ToString()} TotalHours={TotalHours}")]
    public class TimesheetProjectResponse
    {
        public string ProjectId { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string NameDisplay { get => Name == string.Empty ? "<Name is unset>" : Name; }
        public decimal TotalHours { get; set; } = 0;
        public string TotalHoursDisplay { get; set; } = string.Empty;
    }
}
