using EventsAPI.Models;

namespace EventsAPI.Services
{
    /// <summary>Сервис для работы с бронированиями.</summary>
    public class BookingService : IBookingService
    {
        private readonly IEventService _eventService;
        private readonly List<Booking> _bookings = new();

        /// <summary>Создаёт сервис бронирований.</summary>
        /// <param name="eventService">Сервис для проверки существования мероприятий.</param>
        public BookingService(IEventService eventService)
        {
            _eventService = eventService;
        }

        /// <inheritdoc />
        public Task<Booking> CreateBookingAsync(Guid eventId)
        {
            _eventService.GetById(eventId);

            var booking = new Booking(eventId);
            _bookings.Add(booking);

            return Task.FromResult(booking);
        }

        /// <inheritdoc />
        public Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            var booking = _bookings.FirstOrDefault(item => item.Id == bookingId);

            if (booking is null)
                throw new KeyNotFoundException($"Бронь по Id [{bookingId}] не найдена");

            return Task.FromResult(booking);
        }

        /// <inheritdoc />
        public IReadOnlyList<Booking> GetPendingBookings() =>
            _bookings.Where(booking => booking.Status == BookingStatus.Pending).ToList();

        /// <inheritdoc />
        public void ConfirmBooking(Guid bookingId)
        {
            var booking = _bookings.FirstOrDefault(item => item.Id == bookingId)
                ?? throw new KeyNotFoundException($"Бронь по Id [{bookingId}] не найдена");

            if (booking.Status == BookingStatus.Pending)
                booking.Confirm();
        }
    }
}
