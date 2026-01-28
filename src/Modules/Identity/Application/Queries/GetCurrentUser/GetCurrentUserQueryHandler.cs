using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions.Persistence;
using VAlgo.Modules.Identity.Application.Exceptions;
using VAlgo.Modules.Identity.Domain.ValueObjects;

namespace VAlgo.Modules.Identity.Application.Queries.GetCurrentUser
{
    public sealed class GetCurrentUserQueryHandler : IRequestHandler<GetCurrentUserQuery, CurrentUserResponse>
    {
        private readonly IUserRepository _userRepository;

        public GetCurrentUserQueryHandler(IUserRepository userRepository)
            => _userRepository = userRepository;

        public async Task<CurrentUserResponse> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
        {
            var userId = UserId.From(request.UserId);

            var user = await _userRepository.GetByIdAsync(userId, cancellationToken);
            if (user == null)
                throw new UserNotFoundException(request.UserId);

            return new CurrentUserResponse(
                user.Id.Value,
                user.Email.Value,
                user.Username.Value,
                user.Role.ToString(),
                user.IsEmailVerified
            );
        }
    }
}