using Ardalis.SmartEnum;

namespace Playground.Domain.SmartEnum
{
    /// <summary>
    /// Representa los diferentes proveedores de inicio de sesión.
    /// </summary>
    public class TokenTypeSmartEnum : SmartEnum<TokenTypeSmartEnum>
    {
        public static readonly TokenTypeSmartEnum AccessToken = new("AccessToken", 1);

        private TokenTypeSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}