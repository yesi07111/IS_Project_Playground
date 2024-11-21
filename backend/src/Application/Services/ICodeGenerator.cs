namespace Playground.Application.Services
{
    public interface ICodeGenerator
    {
        string GenerateReducedCode(string input, int length = 6);
    }
}