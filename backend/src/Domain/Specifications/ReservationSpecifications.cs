using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    /// <summary>
    /// Especificación para filtrar entidades de reserva.
    /// Proporciona métodos estáticos para crear especificaciones basadas en diferentes propiedades de la reserva.
    /// </summary>
    public class ReservationSpecification : ISpecification<Reservation>
    {
        private readonly Expression<Func<Reservation, bool>> _expression;

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="ReservationSpecification"/>.
        /// </summary>
        /// <param name="expression">La expresión lambda que define la especificación.</param>
        public ReservationSpecification(Expression<Func<Reservation, bool>> expression)
        {
            _expression = expression;
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica AND.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Reservation> And(ISpecification<Reservation> other)
        {
            return new AndSpecification<Reservation>(this, other);
        }

        /// <summary>
        /// Invierte la especificación actual utilizando una operación lógica NOT.
        /// </summary>
        /// <returns>Una nueva especificación que representa la inversión.</returns>
        public ISpecification<Reservation> Not()
        {
            return new NotSpecification<Reservation>(this);
        }

        /// <summary>
        /// Combina la especificación actual con otra especificación utilizando una operación lógica OR.
        /// </summary>
        /// <param name="other">La otra especificación a combinar.</param>
        /// <returns>Una nueva especificación que representa la combinación.</returns>
        public ISpecification<Reservation> Or(ISpecification<Reservation> other)
        {
            return new AndSpecification<Reservation>(this, other);
        }

        /// <summary>
        /// Convierte la especificación en una expresión lambda.
        /// </summary>
        /// <returns>Una expresión lambda que representa la especificación.</returns>
        public Expression<Func<Reservation, bool>> ToExpression()
        {
            return _expression;
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por fecha y hora exactas.
        /// </summary>
        /// <param name="datetime">La fecha y hora de la reserva.</param>
        /// <returns>Una especificación que filtra por fecha y hora.</returns>
        public static ReservationSpecification ByDateTime(DateTime datetime)
        {
            return new ReservationSpecification(reservation => reservation.ActivityDate.DateTime == datetime);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por fecha.
        /// </summary>
        /// <param name="date">La fecha de la reserva.</param>
        /// <returns>Una especificación que filtra por fecha.</returns>
        public static ReservationSpecification ByDate(DateTime date)
        {
            return new ReservationSpecification(reservation => reservation.ActivityDate.DateTime.Date == date.Date);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por hora.
        /// </summary>
        /// <param name="time">La hora de la reserva.</param>
        /// <returns>Una especificación que filtra por hora.</returns>
        public static ReservationSpecification ByTime(int time)
        {
            return new ReservationSpecification(reservation => reservation.ActivityDate.DateTime.Hour == time);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por día de la semana.
        /// </summary>
        /// <param name="day">El día de la semana de la reserva.</param>
        /// <returns>Una especificación que filtra por día de la semana.</returns>
        public static ReservationSpecification ByDayOfWeek(DayOfWeek day)
        {
            return new ReservationSpecification(reservation => reservation.ActivityDate.DateTime.DayOfWeek == day);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por el usuario padre.
        /// </summary>
        /// <param name="parent">El usuario padre asociado a la reserva.</param>
        /// <returns>Una especificación que filtra por usuario padre.</returns>
        public static ReservationSpecification ByParent(string parent)
        {
            return new ReservationSpecification(reservation => reservation.Parent.Id == parent);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por instalación.
        /// </summary>
        /// <param name="facility">La instalación asociada a la reserva.</param>
        /// <returns>Una especificación que filtra por instalación.</returns>
        public static ReservationSpecification ByFacility(Guid facility)
        {
            return new ReservationSpecification(reservation => reservation.Facility.Id == facility);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por cantidad de niños.
        /// </summary>
        /// <param name="ammountOfChildren">La cantidad de niños en la reserva.</param>
        /// <returns>Una especificación que filtra por cantidad de niños.</returns>
        public static ReservationSpecification ByAmmountOfChildren(int ammountOfChildren)
        {
            return new ReservationSpecification(reservation => reservation.AmmountOfChildren == ammountOfChildren);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por estado de la reserva.
        /// </summary>
        /// <param name="reservationState">El estado de la reserva.</param>
        /// <returns>Una especificación que filtra por estado de la reserva.</returns>
        public static ReservationSpecification ByReservationState(string reservationState)
        {
            return new ReservationSpecification(reservation => reservation.ReservationState == reservationState);
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por su fecha de creación.
        /// </summary>
        /// <param name="createdAt">La fecha de creación de la reserva.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de creación.</returns>
        public static ReservationSpecification ByCreatedAt(DateTime createdAt, string comparison = "equal")
        {
            return new ReservationSpecification(DateTimeSpecification<Reservation>.CreateDateComparisonExpression(reservation => reservation.CreatedAt, createdAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por su fecha de actualización.
        /// </summary>
        /// <param name="updateAt">La fecha de actualización de la reserva.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de actualización.</returns>
        public static ReservationSpecification ByUpdateAt(DateTime updateAt, string comparison = "equal")
        {
            return new ReservationSpecification(DateTimeSpecification<Reservation>.CreateDateComparisonExpression(reservation => reservation.UpdateAt, updateAt, comparison));
        }

        /// <summary>
        /// Crea una especificación para filtrar reservas por su fecha de eliminación.
        /// </summary>
        /// <param name="deleteAt">La fecha de eliminación de la reserva.</param>
        /// <param name="comparison">El tipo de comparación a realizar (por defecto es "equal").</param>
        /// <returns>Una especificación que filtra por fecha de eliminación.</returns>
        public static ReservationSpecification ByDeletedAt(DateTime? deleteAt, string comparison = "equal")
        {
            return new ReservationSpecification(DateTimeSpecification<Reservation>.CreateNullableDateComparisonExpression(reservation => reservation.DeletedAt, deleteAt, comparison));
        }
    }
}