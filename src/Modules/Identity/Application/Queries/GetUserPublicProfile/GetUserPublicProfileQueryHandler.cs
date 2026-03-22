using MediatR;
using VAlgo.Modules.Identity.Application.Abstractions;

namespace VAlgo.Modules.Identity.Application.Queries.GetUserPublicProfile
{
    public sealed class GetUserPublicProfileQueryHandler : IRequestHandler<GetUserPublicProfileQuery, UserPublicProfileDto>
    {
        private readonly IUserReadRepository _userReadRepository;

        public GetUserPublicProfileQueryHandler(IUserReadRepository userReadRepository)
        {
            _userReadRepository = userReadRepository;
        }

        public async Task<UserPublicProfileDto> Handle(GetUserPublicProfileQuery request, CancellationToken cancellationToken)
        {
            var user = await _userReadRepository.GetPublicProfileAsync(request.UserName, cancellationToken);

            if (user == null)
                throw new InvalidOperationException("User not found.");

            return user;
        }
    }
}