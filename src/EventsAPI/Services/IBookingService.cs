using EventsAPI.Models;

namespace EventsAPI.Services
{
    /// <summary>Операции создания и получения бронирований.</summary>
    public interface IBookingService
    {
        /// <summary>Создаёт бронь для мероприятия.</summary>
        /// <param name="eventId">Идентификатор мероприятия.</param>
        /// <returns>Созданная бронь.</returns>
        Booking CreateBookingAsync(Guid eventId);
        /// <summary>Находит бронь по идентификатору.</summary>
        /// <param name="bookingId">Идентификатор брони.</param>
        /// <returns>Найденная бронь.</returns>
        Booking GetBookingByIdAsync(Guid bookingId);
    }
}
