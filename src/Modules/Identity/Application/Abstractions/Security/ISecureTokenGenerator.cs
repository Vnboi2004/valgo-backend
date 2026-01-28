namespace VAlgo.Modules.Identity.Application.Abstractions.Security
{
    public interface ISecureTokenGenerator
    {
        string Generate(int length = 64);
    }
}