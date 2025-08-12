using Technical_Test.Application.Base;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.ValueObjects;

namespace TechnicalTest.Application.Timesheets
{
    public class TimesheetsHandlersValidationService(IUserRepository userRepository) : ITimesheetsHandlersValidationService
    {
        private readonly decimal DAILY_HOURS_LIMIT = 10.0m;

        public bool ValidateUser(BaseResponse response, UserId userId)
        {
            try
            {
                var user = userRepository.Get(userId);
            }
            catch (Exception ex)
            {
                response.SetError(ex.Message);
                return false;
            }

            return true;
        }

        public bool ValidateDate(BaseResponse response, TimesheetDate date)
        {
            try
            {
                if (date.Value > DateOnly.FromDateTime(DateTime.Now.Date))
                {
                    response.SetError("Invalid date cannot be in the future");
                    return false;
                }
            }
            catch (Exception ex)
            {
                response.SetError(ex.Message);
                return false;
            }

            return true;
        }

        private bool ValidateTimesheetCommonEntryHours(BaseResponse response, TimesheetHours otherProjDateHours, TimesheetHours validateHours)
        {
            if (validateHours.Value < 0.0m)
            {
                response.SetError("Invalid hours cannot be negative.");
                return false;
            }
            if (validateHours.Value > DAILY_HOURS_LIMIT)
            {
                response.SetError($"Invalid hours cannot be greater than the current daily limit of {DAILY_HOURS_LIMIT}.");
                return false;
            }
            if (otherProjDateHours.Value + validateHours.Value > DAILY_HOURS_LIMIT)
            {
                response.SetError($"Invalid hours across all projects on a date cannot be greater than the current daily limit of {DAILY_HOURS_LIMIT}.");
                return false;
            }

            return true;
        }

        public bool ValidateTimesheetNewEntryHours(BaseResponse response, TimesheetHours otherProjDateHours, TimesheetHours validateHours)
        {
            if (validateHours.Value is null)
            {
                response.SetError("Hour(s) is required on a new entry.");
                return false;
            }
            if (validateHours.Value == 0.0m)
            {
                response.SetError("Invalid hours cannot be zero on a new entry.");
                return false;
            }

            return ValidateTimesheetCommonEntryHours(response, otherProjDateHours, validateHours);
        }

        public bool ValidateTimesheetUpdateEntryHours(BaseResponse response, TimesheetHours otherProjDateHours, TimesheetHours validateHours)
        {
            return ValidateTimesheetCommonEntryHours(response, otherProjDateHours, validateHours);
        }
    }

}
