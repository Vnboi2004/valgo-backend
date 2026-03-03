namespace VAlgo.Modules.Identity.Application.Abstractions.Communication
{
    public interface IEmailSender
    {
        Task SendAsync(string to, string subject, string body, CancellationToken cancellationToken = default);
    }
}