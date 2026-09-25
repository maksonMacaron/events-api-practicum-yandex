using System.Text.Json;
using EventsAPI.Models;
using EventsAPI.Services;

namespace EventsAPI.Tests;

public class BookingServiceTests
{
    [Fact]
    public async Task CreateBookingAsync_ExistingEvent_CreatesPendingBooking()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        var before = DateTime.UtcNow;

        // Act
        var booking = await service.CreateBookingAsync(eventModel.Id);

        // Assert
        Assert.NotEqual(Guid.Empty, booking.Id);
        Assert.Equal(eventModel.Id, booking.EventId);
        Assert.Equal(BookingStatus.Pending, booking.Status);
        Assert.InRange(booking.CreatedAt, before, DateTime.UtcNow);
        Assert.Equal(DateTimeKind.Utc, booking.CreatedAt.Kind);
        Assert.Null(booking.ProcessedAt);

        using var json = JsonDocument.Parse(
            JsonSerializer.Serialize(booking, new JsonSerializerOptions(JsonSerializerDefaults.Web)));
        Assert.Equal("Pending", json.RootElement.GetProperty("status").GetString());
    }

    [Fact]
    public async Task CreateBookingAsync_SameEvent_CreatesUniqueBookings()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);

        // Act
        var first = await service.CreateBookingAsync(eventModel.Id);
        var second = await service.CreateBookingAsync(eventModel.Id);

        // Assert
        Assert.NotEqual(first.Id, second.Id);
    }

    [Fact]
    public async Task CreateBookingAsync_ConcurrentCalls_CreateAllBookings()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);

        // Act
        var bookings = await Task.WhenAll(
            Enumerable.Range(0, 100).Select(_ => service.CreateBookingAsync(eventModel.Id)));

        // Assert
        Assert.Equal(100, bookings.Select(booking => booking.Id).Distinct().Count());
        Assert.Equal(100, service.GetPendingBookings().Count);
    }

    [Fact]
    public async Task CreateBookingAsync_MissingEvent_Throws()
    {
        // Arrange
        var service = new BookingService(new EventService(new List<Event>()));

        // Act
        var action = () => service.CreateBookingAsync(Guid.NewGuid());

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(action);
    }

    [Fact]
    public async Task CreateBookingAsync_DeletedEvent_Throws()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        eventService.Delete(eventModel.Id);

        // Act
        var action = () => service.CreateBookingAsync(eventModel.Id);

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(action);
    }

    [Fact]
    public async Task GetBookingByIdAsync_ExistingBooking_ReturnsBooking()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        var expected = await service.CreateBookingAsync(eventModel.Id);

        // Act
        var actual = await service.GetBookingByIdAsync(expected.Id);

        // Assert
        Assert.Same(expected, actual);
    }

    [Fact]
    public async Task GetBookingByIdAsync_MissingBooking_Throws()
    {
        // Arrange
        var service = new BookingService(new EventService(new List<Event>()));

        // Act
        var action = () => service.GetBookingByIdAsync(Guid.NewGuid());

        // Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(action);
    }

    [Fact]
    public async Task GetBookingByIdAsync_ReflectsConfirmedAndRejectedStatuses()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        var confirmed = await service.CreateBookingAsync(eventModel.Id);
        var rejected = await service.CreateBookingAsync(eventModel.Id);

        // Act
        service.ConfirmBooking(confirmed.Id);
        service.RejectBooking(rejected.Id);
        var confirmedResult = await service.GetBookingByIdAsync(confirmed.Id);
        var rejectedResult = await service.GetBookingByIdAsync(rejected.Id);

        // Assert
        Assert.Equal(BookingStatus.Confirmed, confirmedResult.Status);
        Assert.NotNull(confirmedResult.ProcessedAt);
        Assert.Equal(BookingStatus.Rejected, rejectedResult.Status);
        Assert.NotNull(rejectedResult.ProcessedAt);
        Assert.Empty(service.GetPendingBookings());
    }

    [Fact]
    public async Task ConfirmBooking_AlreadyConfirmed_DoesNotChangeProcessedAt()
    {
        // Arrange
        var eventService = new EventService(new List<Event>());
        var eventModel = eventService.Create(NewEvent());
        var service = new BookingService(eventService);
        var booking = await service.CreateBookingAsync(eventModel.Id);
        service.ConfirmBooking(booking.Id);
        var processedAt = booking.ProcessedAt;

        // Act
        service.ConfirmBooking(booking.Id);

        // Assert
        Assert.Equal(BookingStatus.Confirmed, booking.Status);
        Assert.Equal(processedAt, booking.ProcessedAt);
    }

    [Fact]
    public void ConfirmBooking_MissingBooking_Throws()
    {
        // Arrange
        var service = new BookingService(new EventService(new List<Event>()));

        // Act
        var action = () => service.ConfirmBooking(Guid.NewGuid());

        // Assert
        Assert.Throws<KeyNotFoundException>(action);
    }

    [Fact]
    public void RejectBooking_MissingBooking_Throws()
    {
        // Arrange
        var service = new BookingService(new EventService(new List<Event>()));

        // Act
        var action = () => service.RejectBooking(Guid.NewGuid());

        // Assert
        Assert.Throws<KeyNotFoundException>(action);
    }

    [Fact]
    public void NewBooking_WithEmptyEventId_Throws()
    {
        // Arrange, Act
        var action = () => new Booking(Guid.Empty);

        // Assert
        Assert.Throws<ArgumentException>(action);
    }

    private static Event NewEvent() =>
        new("Тестовое событие", null, new DateTime(2026, 10, 1), new DateTime(2026, 10, 2));
}
