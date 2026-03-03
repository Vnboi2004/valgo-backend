namespace VAlgo.Modules.Identity.Application.Exceptions
{
    public sealed class InvalidEmailVerificationTokenException : Exception
    {
        public InvalidEmailVerificationTokenException()
            : base("Invalid or expired email verification token.") { }
    }
}
