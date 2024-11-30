namespace Playground.Application.Services
{
    /// <summary>
    /// Interfaz para un generador de códigos que proporciona métodos para generar códigos reducidos.
    /// </summary>
    public interface ICodeGenerator
    {
        /// <summary>
        /// Genera un código reducido a partir de una cadena de entrada.
        /// </summary>
        /// <param name="input">La cadena de entrada para generar el código.</param>
        /// <param name="length">La longitud del código generado (por defecto es 6).</param>
        /// <returns>El código reducido generado.</returns>
        string GenerateReducedCode(string input, int length = 6);
    }
}