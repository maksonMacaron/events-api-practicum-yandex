using EventsAPI.Models;

namespace EventsAPI.Services
{
    /// <summary>Операции создания и получения бронирований.</summary>
    public interface IBookingService
    {
        /// <summary>Создаёт бронь для мероприятия.</summary>
        /// <param name="eventId">Идентификатор мероприятия.</param>
        /// <returns>Созданная бронь.</returns>
        Task<Booking> CreateBookingAsync(Guid eventId);
        /// <summary>Находит бронь по идентификатору.</summary>
        /// <param name="bookingId">Идентификатор брони.</param>
        /// <returns>Найденная бронь.</returns>
        Task<Booking> GetBookingByIdAsync(Guid bookingId);

        /// <summary>Возвращает брони, ожидающие обработки.</summary>
        /// <returns>Список ожидающих броней.</returns>
        IReadOnlyList<Booking> GetPendingBookings();

        /// <summary>Подтверждает ожидающую бронь.</summary>
        /// <param name="bookingId">Идентификатор брони.</param>
        void ConfirmBooking(Guid bookingId);

        /// <summary>Отклоняет ожидающую бронь.</summary>
        /// <param name="bookingId">Идентификатор брони.</param>
        void RejectBooking(Guid bookingId);
    }
}
