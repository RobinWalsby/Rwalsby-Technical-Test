using MediatR;
using System.Diagnostics;
using Technical_Test.Application.Base;

namespace TechnicalTest.Application.Users
{
    [DebuggerDisplay("UserId = {UserId.Value}")]
    public class UserRequestQuery : IRequest<UserRequestResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public UserRequestQuery(string userId)
        {
            this.UserId = userId;
        }
    }

    [DebuggerDisplay("UserName = {UserName}")]
    public class UserRequestResponse : BaseResponse
    {
        public string UserId { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
    }
}
