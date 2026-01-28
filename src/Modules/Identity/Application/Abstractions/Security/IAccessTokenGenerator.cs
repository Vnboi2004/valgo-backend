using VAlgo.Modules.Identity.Domain.Aggregates;

namespace VAlgo.Modules.Identity.Application.Abstractions.Security
{
    public interface IAccessTokenGenerator
    {
        string Generate(User user);
    }
}