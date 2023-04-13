using System;

namespace TreeCore.BackEnd.Model.Exceptions
{
    public class ForbiddenException : Exception
    {
        public ForbiddenException(string message) :
            base(message)
        {
        }
    }
}
