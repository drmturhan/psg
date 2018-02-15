using System;

namespace Psg.Api.Base
{
    public class BadRequestError : ArgumentException
    {
        public BadRequestError()
        {
        }

        public BadRequestError(string message) : base(message)
        {
        }
    }
    
}

