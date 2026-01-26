namespace VAlgo.Modules.Identity.Application.Abstractions
{
    public interface ISecureTokenGenerator
    {
        string Generate(int length = 64);
    }
}