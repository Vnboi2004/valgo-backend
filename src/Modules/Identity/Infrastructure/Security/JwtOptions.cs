namespace VAlgo.Modules.Identity.Infrastructure.Security
{
    public sealed class JwtOptions
    {
        public string Issuer { get; init; } = default!;
        public string Audience { get; init; } = default!;
        public string SecretKey { get; init; } = default!;
        public int AccessTokenMinutes { get; init; }
    }
}