using System.Collections.Concurrent;
using EventsAPI.Models;

namespace EventsAPI.Services
{
    /// <summary>Сервис для работы с бронированиями.</summary>
    public class BookingService : IBookingService
    {
        private readonly IEventService _eventService;
        private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

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
            _bookings.TryAdd(booking.Id, booking);

            return Task.FromResult(booking);
        }

        /// <inheritdoc />
        public Task<Booking> GetBookingByIdAsync(Guid bookingId)
        {
            if (!_bookings.TryGetValue(bookingId, out var booking))
                throw new KeyNotFoundException($"Бронь по Id [{bookingId}] не найдена");

            return Task.FromResult(booking);
        }

        /// <inheritdoc />
        public IReadOnlyList<Booking> GetPendingBookings() =>
            _bookings.Values.Where(booking => booking.Status == BookingStatus.Pending).ToList();

        /// <inheritdoc />
        public void ConfirmBooking(Guid bookingId)
        {
            if (!_bookings.TryGetValue(bookingId, out var booking))
                throw new KeyNotFoundException($"Бронь по Id [{bookingId}] не найдена");

            if (booking.Status == BookingStatus.Pending)
                booking.Confirm();
        }

        /// <inheritdoc />
        public void RejectBooking(Guid bookingId)
        {
            if (!_bookings.TryGetValue(bookingId, out var booking))
                throw new KeyNotFoundException($"Бронь по Id [{bookingId}] не найдена");

            if (booking.Status == BookingStatus.Pending)
                booking.Reject();
        }
    }
}
