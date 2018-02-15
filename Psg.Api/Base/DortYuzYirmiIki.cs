using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace Psg.Api.Base
{
    public class DortYuzYirmiIki : ObjectResult
    {
        public DortYuzYirmiIki(ModelStateDictionary modelState) : base(new SerializableError(modelState))
        {
            if (modelState == null)
                throw new ArgumentNullException(nameof(modelState));
            StatusCode = 422;
        }
    }
    
}

