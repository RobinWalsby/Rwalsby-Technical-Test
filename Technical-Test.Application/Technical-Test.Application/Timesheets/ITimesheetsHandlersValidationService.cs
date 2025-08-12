using Technical_Test.Application.Base;
using Technical_Test.Domain.ValueObjects;

namespace TechnicalTest.Application.Timesheets
{
    public interface ITimesheetsHandlersValidationService
    {
        bool ValidateUser(BaseResponse response, UserId userId);

        bool ValidateDate(BaseResponse response, TimesheetDate date);

        bool ValidateTimesheetNewEntryHours(BaseResponse response, TimesheetHours otherProjDateHours, TimesheetHours validateHours);

        bool ValidateTimesheetUpdateEntryHours(BaseResponse response, TimesheetHours otherProjDateHours, TimesheetHours validateHours);
    }   
}
