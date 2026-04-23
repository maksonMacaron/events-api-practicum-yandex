using EventsAPI.DTOs;
using EventsAPI.Models;

namespace EventsAPI.Services
{
    public class EventService : IEventService
    {

        private static List<Event> _events = new List<Event>()
        {
            new Event("Мероприятие #1", "Тут описание", DateTime.Now, DateTime.Now.AddDays(5)),
            new Event("Еще одно какое-то мероприятие", null, DateTime.Now.AddDays(10), DateTime.Now.AddDays(20)),
            new Event("Концерт Сергея Лазарева", null, DateTime.Now.AddDays(15), DateTime.Now.AddDays(15)),
            new Event("Спектакль Горе от ума", null, DateTime.Now.AddDays(17), DateTime.Now.AddDays(18)),
            new Event("Спектакль Алые паруса", null, DateTime.Now.AddDays(22), DateTime.Now.AddDays(23)),
            new Event("Спектакль Мартышка", null, DateTime.Now.AddDays(28), DateTime.Now.AddDays(30)),
            new Event("Спектакль Пикова дама", null, DateTime.Now.AddDays(30), DateTime.Now.AddDays(35)),
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

        public PaginatedResult<Event> GetAll(int page, int pageSize, string? title, DateTime? from, DateTime? to)
        {
            var query = _events.AsEnumerable();
            int total = 0;

            if (!string.IsNullOrWhiteSpace(title))
                query = query.Where(e => e.Title.Contains(title, StringComparison.OrdinalIgnoreCase));

            if (from.HasValue)
                query = query.Where(e => e.StartAt >= from);

            if (to.HasValue)
                query = query.Where(e => e.EndAt <= to);

            total = query.Count();

            query = query.Skip((page - 1) * pageSize).Take(pageSize);

            return new PaginatedResult<Event>()
            {
                Count = query.Count(),
                Total = total,
                Items = query,
                Page = page,
                PageSize = pageSize,
            };
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
