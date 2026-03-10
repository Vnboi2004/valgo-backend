namespace VAlgo.Modules.Identity.Infrastructure.Communication
{
    public sealed class EmailOptions
    {
        public string SmtpHost { get; init; } = default!;
        public int SmtpPort { get; init; }
        public string Username { get; init; } = default!;
        public string Password { get; init; } = default!;
        public string FromEmail { get; init; } = default!;
        public string FromName { get; init; } = default!;
        public bool UseSsl { get; init; } = true;
    }
}
