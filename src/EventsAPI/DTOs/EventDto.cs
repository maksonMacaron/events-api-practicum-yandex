using System.ComponentModel.DataAnnotations;

namespace EventsAPI.DTOs
{
    public class EventDto : IValidatableObject
    {
        public Guid Id { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Название события обязателено для заполнения")]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Название события должно быть от 3 до 100 символов")]
        public string Title { get; set; }

        public string? Description { get; set; }

        [Required(ErrorMessage = "Дата начала обязательна")]
        public DateTime StartAt { get; set; }

        [Required(ErrorMessage = "Дата окончания обязательна")]
        public DateTime EndAt { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (EndAt <= StartAt)
            {
                yield return new ValidationResult(
                    "Дата окончания должна быть позже даты начала",
                    new[] { nameof(EndAt), nameof(StartAt) });
            }
        }
    }
}
