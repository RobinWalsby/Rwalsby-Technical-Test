using MediatR;
using Technical_Test.Domain.Interfaces.Repositories;
using Technical_Test.Domain.ValueObjects;

namespace TechnicalTest.Application.Users
{
    public class UserRequestQueryHandler(IUserRepository userRepository) : IRequestHandler<UserRequestQuery, UserRequestResponse>
    {
        public async Task<UserRequestResponse> Handle(UserRequestQuery request, CancellationToken cancellationToken)
        {
            var response = new UserRequestResponse();
            try
            {
                var userId = UserId.Create(Guid.Parse(request.UserId));
                var user = userRepository.Get(userId);

                response.UserId = user.UserId.Value.ToString();
                response.UserName = user.UserName.Value;

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
