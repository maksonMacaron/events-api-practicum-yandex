using System.ComponentModel.DataAnnotations;

namespace EventsAPI.DTOs
{
    public class EventDto
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Название события обязателено для заполнения")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название события должно быть от 3 до 100 символов")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required]
        public DateTime StartAt { get; set; }

        [Required]
        public DateTime EndAt { get; set; }
    }
}
