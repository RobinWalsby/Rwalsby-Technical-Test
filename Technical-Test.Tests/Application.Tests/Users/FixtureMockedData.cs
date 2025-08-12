using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.Models;
using Technical_Test.Domain.ValueObjects;
using TechnicalTest.Application.Users;

namespace ApplicationTests.Users
{
    [TestClass]
    public class FixtureMockedData
    {
        public static readonly Guid USER_ID = Guid.NewGuid();

        private readonly UserFixture _fixture;

        public class EmptyUserRepository : IUserRepository
        {
            public User Get(UserId id)
            {
                throw new KeyNotFoundException($"User not found with ID {id.Value}.");
            }

            public void Add(User user)
            {
                throw new NotImplementedException();
            }

            public void Delete(UserId id)
            {
                throw new NotImplementedException();
            }
        }

        public class PreDefinedUserRepository : IUserRepository
        {
            public User Get(UserId id)
            {
                var user = User.Create(
                    UserId.Create(FixtureMockedData.USER_ID),
                    UserName.Create("Robin Walsby")
                );

                return user;
            }

            public void Add(User user)
            {
                throw new NotImplementedException();
            }

            public void Delete(UserId id)
            {
                throw new NotImplementedException();
            }
        }

        public class UserFixture
        {
            private IUserRepository? _userRepository = null;

            public async Task<UserRequestResponse> Send(UserRequestQuery query)
            {
                var services = new ServiceCollection();
                var serviceProvider = services
                    .AddLogging()
                    .AddMediatR(cfg => cfg.RegisterServicesFromAssemblies(typeof(UserRequestQuery).Assembly))
                    .AddScoped<IUserRepository>(_ => _userRepository!)
                    .BuildServiceProvider();

                var mediator = serviceProvider.GetRequiredService<IMediator>();

                var response = await mediator.Send(query);

                return response;
            }

            public UserFixture WithPreDefinedUser()
            {
                _userRepository = new PreDefinedUserRepository();
                return this;
            }

            public UserFixture WithEmptyUser()
            {
                _userRepository = new EmptyUserRepository();
                return this;
            }
        }

        public FixtureMockedData()
        {
            _fixture = new UserFixture();
        }

        [TestMethod]
        public async Task UserRequestQueryHandler_ShouldBeTrue_ValidUserResponseStateSuccess()
        {
            var query = new UserRequestQuery(FixtureMockedData.USER_ID.ToString());

            var response = await _fixture
                .WithPreDefinedUser()
                .Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Success);
            Assert.IsTrue(response.UserId == FixtureMockedData.USER_ID.ToString());
        }


        [TestMethod]
        public async Task UserRequestQueryHandler_ShouldBeTrue_UserNotFoundUserResponseStateError()
        {
            var query = new UserRequestQuery(FixtureMockedData.USER_ID.ToString());

            var response = await _fixture
                .WithEmptyUser()
                .Send(query);

            Assert.IsTrue(response.State == Technical_Test.Application.Enum.ResponseState.Error);
            Assert.IsTrue(response.ErrorMessage.Contains("User not found with ID"));
        }
    }
}
