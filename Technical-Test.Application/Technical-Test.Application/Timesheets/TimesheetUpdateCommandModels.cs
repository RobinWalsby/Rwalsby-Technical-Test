using MediatR;
using System.Diagnostics;
using Technical_Test.Application.Base;

namespace TechnicalTest.Application.Timesheets
{
    [DebuggerDisplay("UserId = {UserId} Date = {Date} Hours = {Hours}")]
    public class TimesheetUpdateCommand : IRequest<TimesheetUpdateResponse>
    {
        public string UserId { get; set; } = string.Empty;
        public string ProjectId { get; set; } = string.Empty;
        public string Date { get; set; } = string.Empty;
        public string Hours { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    public class TimesheetUpdateResponse : BaseResponse
    {
        public string TimesheetId { get; set; } = string.Empty;
    }

}
