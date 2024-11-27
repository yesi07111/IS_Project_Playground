//newLaura

using System.Linq.Expressions;
using Playground.Domain.Entities;
using Playground.Domain.Entities.Auth;
using Playground.Domain.Specifications.BaseSpecifications;

namespace Playground.Domain.Specifications
{
    public class ReservationSpecification : ISpecification<Reservation>
    {
        private readonly Expression<Func<Reservation, bool>> _expression;

        public ReservationSpecification(Expression<Func<Reservation, bool>> expression)
        {
            _expression = expression;
        }

        public ISpecification<Reservation> And(ISpecification<Reservation> other)
        {
            return new AndSpecification<Reservation>(this, other);
        }

        public ISpecification<Reservation> Not()
        {
            return new NotSpecification<Reservation>(this);
        }

        public ISpecification<Reservation> Or(ISpecification<Reservation> other)
        {
            return new AndSpecification<Reservation>(this, other);
        }

        public Expression<Func<Reservation, bool>> ToExpression()
        {
            return _expression;
        }

        public static ReservationSpecification ByDateTime(DateTime datetime) //ordenar por fecha y hora
        {
            return new ReservationSpecification(reservation => reservation.Date == datetime);
        }

        public static ReservationSpecification ByDate(DateTime date) //ordenar por fecha
        {
            return new ReservationSpecification(reservation => reservation.Date.Date == date.Date);
        }

        public static ReservationSpecification ByTime(int time) //ordenar por hora
        {
            return new ReservationSpecification(reservation => reservation.Date.Hour == time);
        }

        public static ReservationSpecification ByDayOfWeek(DayOfWeek day) //ordenar por dia de la semana
        {
            return new ReservationSpecification(reservation => reservation.Date.DayOfWeek == day);
        }

        public static ReservationSpecification ByParentId(User parentId) //por padre
        {
            return new ReservationSpecification(reservation => reservation.ParentId == parentId);
        }

        public static ReservationSpecification ByFacility(Facility facility) //por instalacion
        {
            return new ReservationSpecification(reservation => reservation.Facility == facility);
        }

        public static ReservationSpecification ByAmmountOfChildren(int ammountOfChildren) //por cantidad de ninos
        {
            return new ReservationSpecification(reservation => reservation.AmmountOfChildren == ammountOfChildren);
        }

        public static ReservationSpecification ByReservationState(string reservationState) //por estad de la reservacion
        {
            return new ReservationSpecification(reservation => reservation.ReservationState == reservationState);
        }
    }
}