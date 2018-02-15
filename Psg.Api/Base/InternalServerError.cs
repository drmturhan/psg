using System;

namespace Psg.Api.Base
{
    public class InternalServerError : InvalidOperationException
    {
        public InternalServerError()
        {
        }

        public InternalServerError(string message) : base(message)
        {
        }
    }
    
}

