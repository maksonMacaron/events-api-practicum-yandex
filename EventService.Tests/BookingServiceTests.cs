using EventsAPI.Models;
using EventsAPI.Services;
using System.Text.Json;

namespace EventsAPI.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task CreateBookingAsync_ExistingEvent_CreatesPendingBooking()
    {
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        var before = DateTime.UtcNow;

        var booking = await service.CreateBookingAsync(eventModel.Id);

        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal(eventModel.Id, booking.EventId);
        Assert.Equal(BookingStatus.Pending, booking.Status);
        Assert.InRange(booking.CreatedAt, before, DateTime.UtcNow);
        Assert.Equal(DateTimeKind.Utc, booking.CreatedAt.Kind);
        Assert.Null(booking.ProcessedAt);
        Assert.Same(booking, await service.GetBookingByIdAsync(booking.Id));

        using var json = JsonDocument.Parse(JsonSerializer.Serialize(booking, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
        Assert.Equal("Pending", json.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task CreateBookingAsync_SameEvent_CreatesUniqueBookings()
    {
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);

        var first = await service.CreateBookingAsync(eventModel.Id);
        var second = await service.CreateBookingAsync(eventModel.Id);

        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public async Task CreateBookingAsync_MissingOrDeletedEvent_Throws()
    {
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await service.CreateBookingAsync(Guid.NewGuid()));

        eventService.Delete(eventModel.Id);
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await service.CreateBookingAsync(eventModel.Id));
    }

    [Fact]
    public async Task GetBookingByIdAsync_MissingBooking_Throws()
    {
        var service = new BookingService(new EventService(new List<Event>()));

        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await service.GetBookingByIdAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task GetBookingByIdAsync_ReflectsConfirmedAndRejectedStatuses()
    {
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        var confirmed = await service.CreateBookingAsync(eventModel.Id);
        var rejected = await service.CreateBookingAsync(eventModel.Id);

        service.ConfirmBooking(confirmed.Id);
        rejected.Reject();

        var confirmedResult = await service.GetBookingByIdAsync(confirmed.Id);
        var rejectedResult = await service.GetBookingByIdAsync(rejected.Id);
        Assert.Equal(BookingStatus.Confirmed, confirmedResult.Status);
        Assert.NotNull(confirmedResult.ProcessedAt);
        Assert.Equal(BookingStatus.Rejected, rejectedResult.Status);
        Assert.NotNull(rejectedResult.ProcessedAt);
        Assert.Empty(service.GetPendingBookings());
    }

    [Fact]
    public void NewBooking_WithEmptyEventId_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Booking(Guid.Empty));
    }

    private static Event NewEvent() =>
        new("Тестовое событие", null, new DateTime(2026, 10, 1), new DateTime(2026, 10, 2));
}
