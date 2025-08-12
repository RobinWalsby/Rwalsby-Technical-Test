using MediatR;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;

namespace TechnicalTest.Application.Timesheets
{
    public class TimesheetUpdateCommandHandler(ITimesheetsHandlersValidationService timesheetsRequestQueryHandlerService, ITimesheetRepository timesheetRepository) : IRequestHandler<TimesheetUpdateCommand, TimesheetUpdateResponse>
    {
        public async Task<TimesheetUpdateResponse> Handle(TimesheetUpdateCommand request, CancellationToken cancellationToken)
        {
            var response = new TimesheetUpdateResponse();
            try
            {
                var userId = UserId.Create(Guid.Parse(request.UserId));
                var projectId = ProjectId.Create(request.ProjectId);
                var date = TimesheetDate.Create(request.Date);
                var hours = TimesheetHours.Create(request.Hours);

                if (!timesheetsRequestQueryHandlerService.ValidateUser(response, userId))
                {
                    return response;
                }
                if (!timesheetsRequestQueryHandlerService.ValidateDate(response, date))
                {
                    return response;
                }

                var existingProjectEntry = timesheetRepository.GetEntry(userId, projectId, date);
                var existingDateEntries = timesheetRepository.GetEntries(userId, date);
                var existingDateTotalHours = existingDateEntries.Sum(e => e.Hours.Value!.Value);

                if (existingProjectEntry is null) 
                {
                    if (!timesheetsRequestQueryHandlerService.ValidateTimesheetNewEntryHours(response, TimesheetHours.Create(existingDateTotalHours), hours))
                    {
                        return response;
                    }

                    var timesheetId = TimesheetId.Create(Guid.NewGuid());
                    timesheetRepository.Add(TimesheetEntry.Create(
                        timesheetId,
                        userId,
                        projectId,
                        date,
                        hours,
                        TimesheetDescription.Create(request.Description)));

                    response.TimesheetId = timesheetId.Value.ToString();
                }
                else if (hours.Value == 0)
                {
                    timesheetRepository.Delete(existingProjectEntry.Id);
                }
                else if (hours.Value is null)
                {
                    existingProjectEntry.Description = TimesheetDescription.Create(request.Description);
                    timesheetRepository.Update(existingProjectEntry);
                    response.TimesheetId = existingProjectEntry.Id.Value.ToString();
                }
                else
                {
                    var otherProjectDateHours = existingDateTotalHours - existingProjectEntry.Hours.Value!.Value;
                    var updateProjectDateHours = TimesheetHours.Create(hours.Value.Value);
                    if (!timesheetsRequestQueryHandlerService.ValidateTimesheetUpdateEntryHours(response, TimesheetHours.Create(otherProjectDateHours), updateProjectDateHours))
                    {
                        return response;
                    }

                    existingProjectEntry.Hours = updateProjectDateHours;
                    existingProjectEntry.Description = TimesheetDescription.Create(request.Description);
                    timesheetRepository.Update(existingProjectEntry);
                    response.TimesheetId = existingProjectEntry.Id.Value.ToString();
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
