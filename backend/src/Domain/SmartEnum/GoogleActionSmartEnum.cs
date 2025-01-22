using Ardalis.SmartEnum;

namespace Playground.Domain.SmartEnum
{
    /// <summary>
    /// Representa las diferentes acciones a realizar con google.
    /// </summary>
    public class GoogleActionSmartEnum : SmartEnum<GoogleActionSmartEnum>
    {
        public static readonly GoogleActionSmartEnum Register = new("Register", 1);
        public static readonly GoogleActionSmartEnum Login = new("Login", 2);

        private GoogleActionSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}