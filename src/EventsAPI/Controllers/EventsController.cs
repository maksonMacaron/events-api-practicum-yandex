using AutoMapper;
using EventsAPI.Contracts.Responses;
using EventsAPI.DTOs;
using EventsAPI.Models;
using EventsAPI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;

namespace EventsAPI.Controllers
{
    /// <summary>
    /// Контроллер для работы с мероприятиями.
    /// </summary>
    [Route("[controller]")]
    [ApiController]
    public class EventsController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        /// <summary>
        /// Создаёт экземпляр контроллера мероприятий.
        /// </summary>
        /// <param name="eventService">Сервис для работы с мероприятиями.</param>
        /// <param name="mapper">Сервис маппинга DTO и моделей.</param>
        public EventsController(IEventService eventService, IMapper mapper) 
        { 
            _eventService = eventService;
            _mapper = mapper;
        }


        /// <summary>
        /// Получить список всех мероприятий.
        /// </summary>
        /// <param name="page">Номер страницы для пагинации</param>
        /// <param name="pageSize">Размер страницы для пагинации</param>
        /// <param name="title">Наименование мероприятия (регистронезависимый, частичное совпадение)</param>
        /// <param name="from">Дата события, которые начинаются не раньше указанной</param>
        /// <param name="to">Дата событий, которые заканчиваются не позже указанной даты</param>
        /// <returns>Список мероприятий согласно фильтру или все мероприятия</returns>
        [HttpGet]
        public IActionResult GetAll(
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? title = null,
            [FromQuery] DateTime? from = null,
            [FromQuery] DateTime? to = null)
        {
            var serviceResult = _eventService.GetAll(page, pageSize, title, from, to);

            var dtoResult = new PaginatedResult<EventDto>
            {
                Page = serviceResult.Page,
                PageSize = serviceResult.PageSize,
                Total = serviceResult.Total,
                Count = serviceResult.Count,
                Items = _mapper.Map<IEnumerable<EventDto>>(serviceResult.Items)
            };
            
            return Ok(new ApiResult<PaginatedResult<EventDto>>
            {
                Data = dtoResult,
                Message = $"Список всех событий. Всего {dtoResult.Total}",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
            });
        }

        /// <summary>
        /// Получить мероприятие по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор мероприятия.</param>
        /// <returns>Найденное мероприятие или ошибка 404, если мероприятие не существует.</returns>
        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var findModelEvent = _eventService.GetById(id);
                return Ok(new ApiResult<EventDto>
                {
                    Data = _mapper.Map<EventDto>(findModelEvent),
                    Message = $"Событие по Id [{id}] получено",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResult
                {
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                });
            }
        }

        /// <summary>
        /// Создать новое мероприятие.
        /// </summary>
        /// <param name="eventDto">Данные нового мероприятия.</param>
        /// <returns>Созданное мероприятие.</returns>
        [HttpPost]
        public IActionResult Create([FromBody] EventDto eventDto)
        {
            var modelEvent = _mapper.Map<Event>(eventDto);
            var createEventModel = _eventService.Create(modelEvent);

            return CreatedAtAction(
                nameof(GetById),
                new { id = createEventModel.Id },
                new ApiResult<EventDto>
                {
                    Data = _mapper.Map<EventDto>(createEventModel),
                    Message = $"Новое событие успешно создано с Id [{createEventModel.Id}]",
                    StatusCode = System.Net.HttpStatusCode.Created,
                    Success = true,
                });
        }

        /// <summary>
        /// Полностью обновить мероприятие по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор мероприятия.</param>
        /// <param name="eventDto">Новые данные мероприятия.</param>
        /// <returns>Обновлённое мероприятие или ошибка, если мероприятие не найдено.</returns>
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] EventDto eventDto)
        {
            try
            {
                var modelEvent = _mapper.Map<Event>(eventDto);
                var updateEventModel = _eventService.Update(id, modelEvent);

                return Ok(new ApiResult<EventDto>
                {
                    Data = _mapper.Map<EventDto>(updateEventModel),
                    Message = $"Cобытие успешно обновлено по Id [{updateEventModel.Id}]",
                    StatusCode = System.Net.HttpStatusCode.OK,
                    Success = true,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResult
                {
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Success = false,
                });
            }
        }

        /// <summary>
        /// Удалить мероприятие по идентификатору.
        /// </summary>
        /// <param name="id">Идентификатор мероприятия.</param>
        /// <returns>Пустой ответ, если удаление прошло успешно, или ошибка 404.</returns>
        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            try
            {
                _eventService.Delete(id);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResult
                {
                    Success = false,
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                });
            }
        }

    }
}
