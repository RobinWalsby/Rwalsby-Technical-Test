using MediatR;
using System.Globalization;
using Technical_Test.Common.Helpers;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;

namespace TechnicalTest.Application.Timesheets
{
    public class TimesheetsRequestQueryHandler(ITimesheetsHandlersValidationService timesheetsRequestQueryHandlerService, ITimesheetRepository timesheetRepository) : IRequestHandler<TimesheetsRequestQuery, TimesheetsRequestResponse>
    {
        private TimesheetDailyResponse Map(TimesheetEntry entry)
        {
            return new TimesheetDailyResponse
            {
                Id = entry.Id.Value.ToString(),
                ProjectId = entry.ProjectId.Value.ToString(),
                Date = entry.Date.Value.ToString("ddd dd-MMM-yyyy"),
                WeekOfYear = ISOWeek.GetWeekOfYear(entry.Date.ValueDateTime),
                Hours = entry.Hours.Value!.Value,
                HoursDisplay = TimeHelper.HoursFractionalToHoursMinutes(entry.Hours.Value!.Value),
                Description = entry.Description.Value
            };
        }

        public async Task<TimesheetsRequestResponse> Handle(TimesheetsRequestQuery request, CancellationToken cancellationToken)
        {
            var response = new TimesheetsRequestResponse();
            try
            {
                var userId = UserId.Create(Guid.Parse(request.UserId));
                if (!timesheetsRequestQueryHandlerService.ValidateUser(response, userId))
                {
                    return response;
                }

                int? weekNumber = null;
                if (!string.IsNullOrEmpty(request.WeekNumber))
                {
                    weekNumber = int.Parse(request.WeekNumber);
                    if (weekNumber <= 0 || weekNumber > 53)
                    {
                        response.SetError("Week Number is Invalid.");
                        return response;
                    }
                }

                response.DailyTimesheets = [.. timesheetRepository.GetEntries(userId).Select(dt => Map(dt))];
                if (weekNumber is not null)
                {
                    int thisYear = DateTime.Now.Year;
                    response.DailyTimesheets = response.DailyTimesheets.Where(dt => DateTime.Parse(dt.Date).Year == thisYear && dt.WeekOfYear == weekNumber.Value).ToList();
                }

                foreach (var projectHeader in response.DailyTimesheets.DistinctBy(dt => dt.ProjectId))
                {
                    var project = response.DailyTimesheets.Where(dt => dt.ProjectId == projectHeader.ProjectId);
                    response.Projects.Add(new TimesheetProjectResponse {
                        ProjectId = projectHeader.ProjectId,
                        TotalHours = project.Sum(dt => dt.Hours),
                        TotalHoursDisplay = TimeHelper.HoursFractionalToHoursMinutes(project.Sum(dt => dt.Hours)),
                    });
                }

                response.SetSuccess();
            }
            catch (Exception ex)
            {
                response.SetError(ex.Message);
            }

            return response;
        }
    }    
}
