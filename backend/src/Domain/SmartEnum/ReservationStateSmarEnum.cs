using Ardalis.SmartEnum;

namespace Playground.Domain.SmartEnum
{
    /// <summary>
    /// Representa los diferentes estados a de las reservas.
    /// </summary>
    public class ReservationStateSmartEnum : SmartEnum<ReservationStateSmartEnum>
    {
        public static readonly ReservationStateSmartEnum Pendiente = new("Pendiente", 1);
        public static readonly ReservationStateSmartEnum Confirmada = new("Confirmada", 2);
        public static readonly ReservationStateSmartEnum Completada = new("Completada", 3);

        private ReservationStateSmartEnum(string name, int value) : base(name, value)
        {
        }
    }
}