using Ardalis.SmartEnum;

namespace Playground.Domain.SmartEnum
{
    /// <summary>
    /// Representa los diferentes estados a de las reservas.
    /// </summary>
    public class ResourceStateSmartEnum : SmartEnum<ResourceStateSmartEnum>
    {
        public static readonly ResourceStateSmartEnum Bueno = new("Bueno", 1);
        public static readonly ResourceStateSmartEnum Deteriorado = new("Deteriorado", 2);
        public static readonly ResourceStateSmartEnum Roto = new("Roto", 3);

        private ResourceStateSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}