using System.Diagnostics;
using Technical_Test.Application.Enum;

namespace Technical_Test.Application.Base
{
    [DebuggerDisplay("ErrorMessage = {ErrorMessage}")]
    public class BaseResponse
    {
        public ResponseState State { get; set; } = ResponseState.None;

        public string ErrorMessage { get; set; } = string.Empty;

        public bool IsUnProcessed { get => State == ResponseState.None; }

        public bool IsSuccess { get => State == ResponseState.Success; }

        public bool IsError { get => State == ResponseState.Error; }

        public void SetSuccess()
        {
            State = ResponseState.Success;
            ErrorMessage = string.Empty;
        }

        public void SetError(string errorMessage)
        {
            State = ResponseState.Error;
            ErrorMessage = errorMessage;
        }
    }
}
