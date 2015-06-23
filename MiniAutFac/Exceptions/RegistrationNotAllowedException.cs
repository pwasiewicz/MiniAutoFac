namespace MiniAutFac.Exceptions
{
    using System;

    public class RegistrationNotAllowedException : Exception
    {
        public RegistrationNotAllowedException() { }
        public RegistrationNotAllowedException(string message) : base(message) { }
        public RegistrationNotAllowedException(string message, Exception inner) : base(message, inner) { }
    }
}
