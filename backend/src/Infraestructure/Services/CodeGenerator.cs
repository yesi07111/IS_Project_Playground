using System.Security.Cryptography;
using System.Text;
using Playground.Application.Services;

namespace Playground.Infraestructure.Services;
public class CodeGenerator : ICodeGenerator
{
    public string GenerateReducedCode(string input, int length = 6)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        string base64String = Convert.ToBase64String(hashBytes);

        return base64String[..length];
    }
}