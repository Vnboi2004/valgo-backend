using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Policies;
using VAlgo.Modules.Identity.Domain.Aggregates;
using VAlgo.Modules.Identity.Domain.Exceptions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Domain.Services
{
    public sealed class LoginPolicy : ILoginPolicy
    {
        private const int MaxFailedAttempts = 5;
        private static readonly TimeSpan FailureWindow = TimeSpan.FromMinutes(10);
        private static readonly TimeSpan LockDuration = TimeSpan.FromMinutes(15);

        private readonly ILoginAttemptRepository _loginAttemptRepository;

        public LoginPolicy(ILoginAttemptRepository loginAttemptRepository)
        {
            _loginAttemptRepository = loginAttemptRepository;
        }

        public async Task EvaluateAsync(User user, IClock clock, CancellationToken cancellationToken)
        {
            if (user.IsLocked(clock))
                throw new UserLockedException();

            var since = clock.UtcNow.Subtract(FailureWindow);

            var failedAttempts = await _loginAttemptRepository.CountFailedAttemptsAsync(user.Id, since, cancellationToken);

            if (failedAttempts >= MaxFailedAttempts)
            {
                user.Lock(clock.UtcNow.Add(LockDuration));
                throw new UserLockedException();
            }
        }
    }
}