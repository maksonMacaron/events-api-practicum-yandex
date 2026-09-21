using EventsAPI.Models;

namespace EventsAPI.Services;

/// <summary>Периодически обрабатывает ожидающие бронирования.</summary>
public class BookingProcessingService : BackgroundService
{
    private readonly IBookingService _bookingService;
    private readonly ILogger<BookingProcessingService> _logger;

    /// <summary>Создаёт фоновый сервис обработки бронирований.</summary>
    /// <param name="bookingService">Сервис бронирований.</param>
    /// <param name="logger">Сервис логирования.</param>
    public BookingProcessingService(IBookingService bookingService, ILogger<BookingProcessingService> logger)
    {
        _bookingService = bookingService;
        _logger = logger;
    }

    /// <inheritdoc />
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("BookingProcessingService запущен");

        using var timer = new PeriodicTimer(TimeSpan.FromSeconds(1));

        try
        {
            while (await timer.WaitForNextTickAsync(stoppingToken))
            {
                IReadOnlyList<Booking> pendingBookings;
                try
                {
                    pendingBookings = _bookingService.GetPendingBookings();
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Не удалось получить ожидающие бронирования");
                    continue;
                }

                foreach (var booking in pendingBookings)
                {
                    try
                    {
                        await Task.Delay(TimeSpan.FromSeconds(2), stoppingToken);
                        _bookingService.ConfirmBooking(booking.Id);
                        _logger.LogInformation("Бронь {BookingId} подтверждена", booking.Id);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        return;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "Не удалось обработать бронь {BookingId}", booking.Id);
                    }
                }
            }
        }
        catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
        {
            _logger.LogInformation("BookingProcessingService остановлен");
        }
    }
}
