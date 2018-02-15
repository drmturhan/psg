using System;

namespace Psg.Api.Base
{
    public class UnauthorizedError : ArgumentException
    {
        public UnauthorizedError()
        {
        }

        public UnauthorizedError(string message) : base(message)
        {
        }
    }
}

