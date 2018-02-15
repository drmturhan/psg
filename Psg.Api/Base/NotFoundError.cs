using System;

namespace Psg.Api.Base
{
    public class NotFoundError : ArgumentException
    {
        public NotFoundError()
        {
        }

        public NotFoundError(string message) : base(message)
        {
        }
    }
}

