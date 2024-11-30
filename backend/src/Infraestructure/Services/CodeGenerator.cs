using System.Security.Cryptography;
using System.Text;
using Playground.Application.Services;

namespace Playground.Infraestructure.Services;

/// <summary>
/// Servicio para generar códigos reducidos a partir de cadenas de texto.
/// Utiliza SHA256 para crear un hash y lo convierte a una cadena Base64.
/// </summary>
public class CodeGenerator : ICodeGenerator
{
    /// <summary>
    /// Genera un código reducido a partir de una entrada de texto.
    /// </summary>
    /// <param name="input">La cadena de texto de entrada para generar el código.</param>
    /// <param name="length">La longitud del código reducido (por defecto es 6).</param>
    /// <returns>Un código reducido como una cadena de texto.</returns>
    public string GenerateReducedCode(string input, int length = 6)
    {
        using var sha256 = SHA256.Create();
        byte[] hashBytes = SHA256.HashData(Encoding.UTF8.GetBytes(input));

        string base64String = Convert.ToBase64String(hashBytes);

        return base64String[..length];
    }
}