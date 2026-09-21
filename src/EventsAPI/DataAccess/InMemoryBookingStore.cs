using System.Collections.Concurrent;
using EventsAPI.Models;

namespace EventsAPI.DataAccess;

/// <summary>
/// Общее для приложения хранилище бронирований в памяти.
/// </summary>
public class InMemoryBookingStore
{
    private readonly ConcurrentDictionary<Guid, Booking> _bookings = new();

    /// <summary>Сохраняет новую бронь.</summary>
    /// <param name="booking">Бронь для сохранения.</param>
    /// <exception cref="InvalidOperationException">Бронь с таким идентификатором уже существует.</exception>
    public void Add(Booking booking)
    {
        ArgumentNullException.ThrowIfNull(booking);

        if (!_bookings.TryAdd(booking.Id, booking))
            throw new InvalidOperationException($"Бронь по Id [{booking.Id}] уже существует");
    }

    /// <summary>Находит бронь по идентификатору.</summary>
    /// <param name="id">Идентификатор брони.</param>
    /// <returns>Бронь или <see langword="null" />, если она не найдена.</returns>
    public Booking? GetById(Guid id) =>
        _bookings.TryGetValue(id, out var booking) ? booking : null;

    /// <summary>Возвращает брони, ожидающие обработки.</summary>
    /// <returns>Снимок списка ожидающих броней.</returns>
    public IReadOnlyCollection<Booking> GetPending() =>
        _bookings.Values.Where(booking => booking.Status == BookingStatus.Pending).ToArray();
}
