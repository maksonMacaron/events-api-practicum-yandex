using EventsAPI.Models;

namespace EventsAPI.Services
{
    public class EventService : IEventService
    {

        private static List<Event> _events = new List<Event>()
        {
            new Event("Мероприятие #1", "Тут описание", DateTime.Now, DateTime.Now.AddDays(5)),
            new Event("Еще одно какое-то мероприятие", null, DateTime.Now.AddDays(10), DateTime.Now.AddDays(20)),
        };

        public Event Create(Event item)
        {
            Event eventNew = new Event(item.Title, item.Description, item.StartAt, item.EndAt);
            _events.Add(eventNew);
            return eventNew;
        }

        public void Delete(Guid id)
        {
            Event? findEvent = GetById(id);
            _events.Remove(findEvent);
        }

        public IEnumerable<Event> GetAll()
        {
            return _events;
        }

        public Event GetById(Guid id)
        {
            Event? findEvent = _events.FirstOrDefault(e => e.Id == id);
            if (findEvent == null)
                throw new KeyNotFoundException($"Событие по Id [{id}] не найдено");
            else return findEvent;
        }

        public Event Update(Guid id, Event item)
        {
            Event? findEvent = GetById(id);
            findEvent.Title = item.Title;
            findEvent.Description = item.Description;
            findEvent.StartAt = item.StartAt;
            findEvent.EndAt = item.EndAt;
            return findEvent;
        }
    }
}
