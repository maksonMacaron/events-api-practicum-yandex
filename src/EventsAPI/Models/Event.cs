using System.Diagnostics.CodeAnalysis;

namespace EventsAPI.Models
{
    /// <summary>
    /// Модель мероприятия.
    /// </summary>
    public class Event
    {
        /// <summary>
        /// Уникальный идентификатор мероприятия.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Название мероприятия.
        /// </summary>
        public required string Title { get; set; }

        /// <summary>
        /// Описание мероприятия.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        /// Дата и время начала мероприятия.
        /// </summary>
        public DateTime StartAt { get; set; }

        /// <summary>
        /// Дата и время окончания мероприятия.
        /// </summary>
        public DateTime EndAt { get; set; }

        /// <summary>
        /// Создаёт пустую модель мероприятия.
        /// </summary>
        public Event() { }

        /// <summary>
        /// Создаёт новое мероприятие с указанными параметрами.
        /// </summary>
        /// <param name="title">Название мероприятия.</param>
        /// <param name="description">Описание мероприятия.</param>
        /// <param name="startAt">Дата и время начала.</param>
        /// <param name="endAt">Дата и время окончания.</param>
        [SetsRequiredMembers]
        public Event(string title, string? description, DateTime startAt, DateTime endAt)
        {
            Id = Guid.NewGuid();
            Title = title;
            Description = description;
            StartAt = startAt;
            EndAt = endAt;
        }
    }
}