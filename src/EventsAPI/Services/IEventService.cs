using EventsAPI.Models;

namespace EventsAPI.Services
{
    public interface IEventService
    {
        IEnumerable<Event> GetAll(string? title, DateTime? from, DateTime? to);
        Event GetById(Guid id);
        void Delete(Guid id);
        Event Create(Event item);
        Event Update(Guid id, Event item);
    }
}
