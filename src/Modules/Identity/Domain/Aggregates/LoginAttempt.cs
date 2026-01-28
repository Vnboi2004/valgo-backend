using VAlgo.Modules.Identity.Domain.Enums;
using VAlgo.Modules.Identity.Domain.ValueObjects;
using VAlgo.SharedKernel.Abstractions;
using VAlgo.SharedKernel.Time;

namespace VAlgo.Modules.Identity.Domain.Aggregates
{
    public sealed class LoginAttempt : AggregateRoot<LoginAttemptId>
    {
        public UserId? UserId { get; private set; }
        public Email? Email { get; private set; }
        public LoginResult Result { get; private set; }
        public LoginFailureReason? FailureReason { get; private set; }
        public DateTimeOffset OccurredAt { get; private set; }

        private LoginAttempt() { }

        private LoginAttempt(
            LoginAttemptId id,
            UserId? userId,
            Email? email,
            LoginResult result,
            LoginFailureReason? failureReason,
            DateTimeOffset occurredAt
        )
       : base(id)
        {
            if (userId == null && email == null)
                throw new InvalidOperationException("LoginAttempt must have either UserId or Email.");

            if (userId == null && email == null)
                throw new InvalidOperationException("LoginAttempt cannot have both UserId and Email.");

            if (result == LoginResult.Failed && failureReason == null)
                throw new InvalidOperationException("Failed login attempt must have a failure reason.");

            if (result == LoginResult.Success && failureReason == null)
                throw new InvalidOperationException("Successful login attempt cannot have a failure reason.");

            UserId = userId;
            Email = email;
            Result = result;
            FailureReason = failureReason;
            OccurredAt = occurredAt;
        }

        public static LoginAttempt Success(UserId userId, IClock clock)
        => new(LoginAttemptId.New(), userId, null, LoginResult.Success, null, clock.UtcNow);

        public static LoginAttempt FailedByEmail(Email email, LoginFailureReason reason, IClock clock)
            => new(LoginAttemptId.New(), null, email, LoginResult.Failed, reason, clock.UtcNow);

        public static LoginAttempt FailedByUser(UserId userId, LoginFailureReason reason, IClock clock)
            => new(LoginAttemptId.New(), userId, null, LoginResult.Failed, reason, clock.UtcNow);
    }
}