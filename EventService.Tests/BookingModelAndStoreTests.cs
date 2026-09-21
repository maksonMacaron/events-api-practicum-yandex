using EventsAPI.DataAccess;
using EventsAPI.Models;

namespace EventsAPI.Tests;

public class BookingModelAndStoreTests
{
    [Fact]
    public void NewBooking_HasUniqueIdPendingStatusAndUtcCreationTime()
    {
        var eventId = Guid.NewGuid();
        var before = DateTime.UtcNow;

        var first = new Booking(eventId);
        var second = new Booking(eventId);

        Assert.NotEqual(Guid.Empty, first.Id);
        Assert.NotEqual(first.Id, second.Id);
        Assert.Equal(eventId, first.EventId);
        Assert.Equal(BookingStatus.Pending, first.Status);
        Assert.InRange(first.CreatedAt, before, DateTime.UtcNow);
        Assert.Equal(DateTimeKind.Utc, first.CreatedAt.Kind);
        Assert.Null(first.ProcessedAt);
    }

    [Fact]
    public void NewBooking_WithEmptyEventId_Throws()
    {
        Assert.Throws<ArgumentException>(() => new Booking(Guid.Empty));
    }

    [Fact]
    public void Store_RetainsBookingAndReflectsStatusChanges()
    {
        var store = new InMemoryBookingStore();
        var booking = new Booking(Guid.NewGuid());

        store.Add(booking);
        Assert.Same(booking, store.GetById(booking.Id));
        Assert.Contains(booking, store.GetPending());

        booking.Confirm();

        Assert.Equal(BookingStatus.Confirmed, store.GetById(booking.Id)?.Status);
        Assert.NotNull(store.GetById(booking.Id)?.ProcessedAt);
        Assert.DoesNotContain(booking, store.GetPending());
        Assert.Null(store.GetById(Guid.NewGuid()));
    }
}
