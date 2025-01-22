using Ardalis.SmartEnum;

namespace Playground.Domain.SmartEnum
{
    /// <summary>
    /// Representa los diferentes proveedores de inicio de sesión.
    /// </summary>
    public class LoginProviderSmartEnum : SmartEnum<LoginProviderSmartEnum>
    {
        public static readonly LoginProviderSmartEnum Default = new("Default", 1);

        private LoginProviderSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}