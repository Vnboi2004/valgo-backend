namespace VAlgo.SharedKernel.Abstractions
{
    public interface ICurrentUserService
    {
        Guid UserId { get; }
        bool IsAuthenticated { get; }
        bool IsInRole(string role);
    }
}