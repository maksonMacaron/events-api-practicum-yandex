using EventsAPI.Models;
using EventsAPI.Services;
using Microsoft.Extensions.Logging.Abstractions;

namespace EventsAPI.Tests;

public class BookingProcessingServiceTests
{
    [Fact]
    public async Task BackgroundService_ConfirmsPendingBookingAfterDelay()
    {
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(new Event(
            "Тестовое событие", null, new DateTime(2026, 10, 1), new DateTime(2026, 10, 2)));
        var bookingService = new BookingService(eventService);
        using var worker = new BookingProcessingService(
            bookingService, NullLogger<BookingProcessingService>.Instance);

        await worker.StartAsync(CancellationToken.None);
        try
        {
            var booking = await bookingService.CreateBookingAsync(eventModel.Id);
            Assert.Equal(BookingStatus.Pending, booking.Status);
            Assert.Null(booking.ProcessedAt);

            using var timeout = new CancellationTokenSource(TimeSpan.FromSeconds(8));
            while (booking.Status == BookingStatus.Pending)
                await Task.Delay(100, timeout.Token);

            Assert.Equal(BookingStatus.Confirmed, (await bookingService.GetBookingByIdAsync(booking.Id)).Status);
            Assert.NotNull(booking.ProcessedAt);
            Assert.True(booking.ProcessedAt >= booking.CreatedAt);
        }
        finally
        {
            await worker.StopAsync(CancellationToken.None);
        }
    }
}
