using EventsAPI.Models;

namespace EventsAPI.Services
{
    /// <summary>Сервис для работы с бронированиями.</summary>
    public class BookingService : IBookingService
    {
        /// <inheritdoc />
        public Booking CreateBookingAsync(Guid eventId)
        {
            throw new NotImplementedException();
        }

        /// <inheritdoc />
        public Booking GetBookingByIdAsync(Guid bookingId)
        {
            throw new NotImplementedException();
        }
    }
}
