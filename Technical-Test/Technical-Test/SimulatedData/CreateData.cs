
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Infrastructure.Repositories;

namespace Technical_Test.SimulateData
{
    public static class CreateData
    {
        public static IServiceProvider CreateTestData(this IServiceProvider services)
        {
            var USER_ID = Guid.Parse("b1f8c5d2-3e4f-4c5a-9b6d-7e8f9a0b1c2d");
            var PROJECT_ID = Guid.Parse("c2d3e4f5-6a7b-8c9d-0e1f-2a3b4c5d6e7f");

            var userRepository = services.GetService<IUserRepository>();
            var timesheetRepository = services.GetService<ITimesheetRepository>();

            userRepository?.Add(Domain.Models.User.Create(
                Domain.ValueObjects.UserId.Create(USER_ID),
                Domain.ValueObjects.UserName.Create("Robin Walsby")
            ));

            timesheetRepository?.Add(Domain.Models.TimesheetEntry.Create(
                Domain.ValueObjects.TimesheetId.Create(Guid.NewGuid()),
                Domain.ValueObjects.UserId.Create(USER_ID),
                Domain.ValueObjects.ProjectId.Create(PROJECT_ID),
                Domain.ValueObjects.TimesheetDate.Create(DateOnly.FromDateTime(DateTime.Now.Date).AddDays(-27)),
                Domain.ValueObjects.TimesheetHours.Create(7.5m),
                Domain.ValueObjects.TimesheetDescription.Create("Current Project"))
            );

            timesheetRepository?.Add(Domain.Models.TimesheetEntry.Create(
                Domain.ValueObjects.TimesheetId.Create(Guid.NewGuid()),
                Domain.ValueObjects.UserId.Create(USER_ID),
                Domain.ValueObjects.ProjectId.Create(PROJECT_ID),
                Domain.ValueObjects.TimesheetDate.Create(DateOnly.FromDateTime(DateTime.Now.Date).AddDays(-4)),
                Domain.ValueObjects.TimesheetHours.Create(5.0m),
                Domain.ValueObjects.TimesheetDescription.Create("Current Project"))
            );

            timesheetRepository?.Add(Domain.Models.TimesheetEntry.Create(
                Domain.ValueObjects.TimesheetId.Create(Guid.NewGuid()),
                Domain.ValueObjects.UserId.Create(USER_ID),
                Domain.ValueObjects.ProjectId.Create(Guid.NewGuid()),
                Domain.ValueObjects.TimesheetDate.Create(DateOnly.FromDateTime(DateTime.Now.Date).AddDays(-3)),
                Domain.ValueObjects.TimesheetHours.Create(3.5m),
                Domain.ValueObjects.TimesheetDescription.Create("Other Project"))
            );

            timesheetRepository?.Add(Domain.Models.TimesheetEntry.Create(
                Domain.ValueObjects.TimesheetId.Create(Guid.NewGuid()),
                Domain.ValueObjects.UserId.Create(USER_ID),
                Domain.ValueObjects.ProjectId.Create(PROJECT_ID),
                Domain.ValueObjects.TimesheetDate.Create(DateOnly.FromDateTime(DateTime.Now.Date).AddDays(-3)),
                Domain.ValueObjects.TimesheetHours.Create(4.0m),
                Domain.ValueObjects.TimesheetDescription.Create("Current Project"))
            );

            return services;
        }

    }
}