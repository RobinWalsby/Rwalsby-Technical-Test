using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;
using Technical_Test.Infrastructure.Repositories;
using TechnicalTest.Application.Timesheets;

namespace ApplicationTests.Timesheet
{
    [TestClass]
    public class CreatedRepositoryData
    {
        private readonly Guid USER_ID = Guid.NewGuid();
        private readonly Guid PROJECT_ID = Guid.NewGuid();
        private readonly DateOnly ENTRY_DATE = DateOnly.FromDateTime(DateTime.Now.Date).AddDays(-7);
        private IUserRepository _userRepository { get; set; }
        private ITimesheetRepository _timesheetRepository { get; set; }
        private IMediator _mediator { get; set; }

        public CreatedRepositoryData()
        {         
            _userRepository = new UserRepository();
            _timesheetRepository = new TimesheetRepository();

            var services = new ServiceCollection();
            var serviceProvider = services
                .AddLogging()
                .AddSingleton<IUserRepository>(_userRepository)
                .AddSingleton<ITimesheetRepository>(_timesheetRepository)
                .AddSingleton<ITimesheetsHandlersValidationService>(new TimesheetsHandlersValidationService(_userRepository))
                .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(TimesheetsRequestQuery).Assembly))
                .BuildServiceProvider();

            _mediator = serviceProvider.GetRequiredService<IMediator>();
        }

        private TimesheetUpdateCommand GetBasicTimesheetUpdateCommand(string description)
        {
            return new TimesheetUpdateCommand()
            {
                UserId = USER_ID.ToString(),
                ProjectId = PROJECT_ID.ToString(),
                Date = ENTRY_DATE.ToString("dd-MMM-yyyy"),
                Hours = "0",
                Description = description
            };
        }

        [TestInitialize]
        public void Initialize()
        {
            _userRepository.Add(User.Create(
                 UserId.Create(USER_ID),
                 UserName.Create("Robin Walsby")));

            _timesheetRepository.Add(TimesheetEntry.Create(
                TimesheetId.Create(Guid.NewGuid()),
                UserId.Create(USER_ID),
                ProjectId.Create(PROJECT_ID),
                TimesheetDate.Create(ENTRY_DATE),
                TimesheetHours.Create(7.5m),
                TimesheetDescription.Create(string.Empty)));
        }

        [TestCleanup]
        public void Cleanup()
        {
            _timesheetRepository.DeleteAll(UserId.Create(USER_ID));
            _userRepository.Delete(UserId.Create(USER_ID));
        }


        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_ValidUserSuccess()
        {
            var query = new TimesheetsRequestQuery(USER_ID.ToString());
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
            Assert.IsTrue(response.DailyTimesheets.Count() == 1);
        }

        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_ValidProjectSuccess()
        {
            _timesheetRepository.Add(TimesheetEntry.Create(
                    TimesheetId.Create(Guid.NewGuid()),
                    UserId.Create(USER_ID),
                    ProjectId.Create(PROJECT_ID),
                    TimesheetDate.Create(ENTRY_DATE.AddDays(-1)),
                    TimesheetHours.Create(3.0m),
                    TimesheetDescription.Create("ValidProjectSuccess")));

            var query = new TimesheetsRequestQuery(USER_ID.ToString());
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
            Assert.IsTrue(response.DailyTimesheets.Count() == 2);
            Assert.IsTrue(response.Projects.Count() == 1);
            Assert.IsTrue(response.Projects[0].TotalHours == 10.5m);
        }

        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_ValidWeekNumberSuccess()
        {
            var query = new TimesheetsRequestQuery(USER_ID.ToString());
            query.WeekNumber = "1";
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
            Assert.IsTrue(response.DailyTimesheets.Count() == 0);
        }

        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_InvalidWeekNumberError()
        {
            var query = new TimesheetsRequestQuery(USER_ID.ToString());
            query.WeekNumber = "0";
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Week Number is Invalid"));
        }

        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_DuplicateEntryUpdateSuccess()
        {
            var command = GetBasicTimesheetUpdateCommand("DuplicateEntryUpdateSuccess");
            command.Date = ENTRY_DATE.ToString("dd-MMM-yyyy");
            command.Hours = "1.0";
            await _mediator.Send(command);

            var query = new TimesheetsRequestQuery(USER_ID.ToString());
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
            Assert.IsTrue(response.DailyTimesheets.Count() == 1);
            Assert.IsTrue(response.Projects.Count() == 1);
            Assert.IsTrue(response.Projects[0].TotalHours == 1m);
        }


        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_DuplicateEntryDeleteSuccess()
        {
            var command = GetBasicTimesheetUpdateCommand("DuplicateEntryDeleteSuccess");
            command.Date = ENTRY_DATE.ToString("dd-MMM-yyyy");
            command.Hours = "0";
            await _mediator.Send(command);

            var query = new TimesheetsRequestQuery(USER_ID.ToString());
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
            Assert.IsTrue(response.DailyTimesheets.Count() == 0);
            Assert.IsTrue(response.Projects.Count() == 0);
        }

        [TestMethod]
        public async Task TimesheetsRequestQuery_ShouldBeTrue_InvalidUserError()
        {
            var query = new TimesheetsRequestQuery(Guid.Parse("b1f8c5d2-3e4f-4c5a-9b6d-7e8f9a0b1d2d").ToString());
            var response = await _mediator.Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("User not found with ID"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_ValidNewEntrySuccess()
        {
            var command = GetBasicTimesheetUpdateCommand("Add 1 hour to Project New Date");
            command.Hours = "1";
            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_InvalidNewEntryMissingHoursError()
        {
            var command = GetBasicTimesheetUpdateCommand("Missing hours in new entry");
            command.Date = ENTRY_DATE.AddDays(-20).ToString("dd-MMM-yyyy");
            command.Hours = "";
            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Hour(s) is required on a new entry"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_ZeroHoursNewEntryError()
        {
            var command = GetBasicTimesheetUpdateCommand("Zero hours in new entry");
            command.Date = ENTRY_DATE.AddDays(-22).ToString("dd-MMM-yyyy");
            command.Hours = "0";
            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid hours cannot be zero on a new entry"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_MultipleProjectsSameDateError()
        {
            _timesheetRepository.Add(TimesheetEntry.Create(
                    TimesheetId.Create(Guid.NewGuid()),
                    UserId.Create(USER_ID),
                    ProjectId.Create(Guid.NewGuid()),
                    TimesheetDate.Create(ENTRY_DATE),
                    TimesheetHours.Create(3.0m),
                    TimesheetDescription.Create("Other project same date")));

            var command = GetBasicTimesheetUpdateCommand("MultipleProjectsSameDateError");
            command.Date = ENTRY_DATE.ToString("dd-MMM-yyyy");
            command.Hours = "8";
            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid hours across all projects on a date cannot be greater than the current daily limit"));

        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_InvalidDailyLimitNewEntryError()
        {
            var command = GetBasicTimesheetUpdateCommand("Set 11 hours to new entry");
            command.Date = ENTRY_DATE.AddDays(-22).ToString("dd-MMM-yyyy");
            command.Hours = "11";
            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid hours cannot be greater than the current daily limit"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_InvalidDailyLimitExistingEntryError()
        {
            var command = GetBasicTimesheetUpdateCommand("Set 11 hours to existing entry");
            command.Hours = "11";
            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid hours cannot be greater than the current daily limit"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_InvalidDateError()
        {
            var command = GetBasicTimesheetUpdateCommand("Add 1 hours to Project Invalid Date");
            command.Date = "30-Feb-2025";

            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("TimesheetDate must be a valid date"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_TooEarlyDateError()
        {
            var command = GetBasicTimesheetUpdateCommand("Add 1 hours to Project Too Early Date");
            command.Date = "27-Feb-2022";

            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("TimesheetDate must be a valid past date"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_InvalidHoursForADayError()
        {
            var command = GetBasicTimesheetUpdateCommand("InvalidHoursForADayError");
            command.Hours = "25";

            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid hours for a day cannot not exceed 24"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_InvalidHoursForEntryError()
        {
            var command = GetBasicTimesheetUpdateCommand("InvalidHourForADayError");
            command.Date = DateTime.Now.Date.AddDays(-50).ToString("dd-MMM-yyyy");
            command.Hours = "10.5";

            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid hours cannot be greater than the current daily limit of 10.0"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_FutureDateError()
        {
            var command = GetBasicTimesheetUpdateCommand("Future Date");
            command.Date = DateTime.Now.Date.AddDays(1).ToString("dd-MMM-yyyy");

            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("Invalid date cannot be in the future"));
        }

        [TestMethod]
        public async Task TimesheetUpdateCommand_ShouldBeTrue_ValidDateTodaySuccess()
        {
            var command = GetBasicTimesheetUpdateCommand("Todays Date");
            command.Date = DateTime.Now.Date.ToString("dd-MMM-yyyy");
            command.Hours = "5.5";

            var response = await _mediator.Send(command);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
        }
    }
}
