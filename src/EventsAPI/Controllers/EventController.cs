using AutoMapper;
using EventsAPI.Contracts.Responses;
using EventsAPI.DTOs;
using EventsAPI.Models;
using EventsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using System.Collections;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private readonly IEventService _eventService;
        private readonly IMapper _mapper;

        public EventController(IEventService eventService, IMapper mapper) 
        { 
            _eventService = eventService;
            _mapper = mapper;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var dtos = _mapper.Map<IEnumerable<EventDto>>(_eventService.GetAll());
            return Ok(new ApiResult<IEnumerable<EventDto>>
            {
                Data = dtos,
                Message = "Получаем список всех событий",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
            });
        }

        [HttpGet("{id}")]
        public IActionResult GetById([FromRoute] Guid id)
        {
            try
            {
                var findModelEvent = _eventService.GetById(id);
                return Ok(new ApiResult<EventDto>
                {
                    Data = _mapper.Map<EventDto>(findModelEvent),
                    Message = $"Событие по Id [{id.ToString()}] получено",
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Success = true,
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ApiResult
                {
                    Message = ex.Message,
                    StatusCode = System.Net.HttpStatusCode.NotFound,
                    Success = true,
                });
            }
        }

        [HttpPost()]
        public IActionResult Create([FromBody] EventDto eventDto)
        {
            var modelEvent = _mapper.Map<Event>(eventDto);
            var createEventModel = _eventService.Create(modelEvent);

            return Created("Create", new ApiResult<EventDto>
            {
                Data = _mapper.Map<EventDto>(createEventModel),
                Message = $"Новое событие успешно создано с Id [{createEventModel.Id}]",
                StatusCode = System.Net.HttpStatusCode.OK,
                Success = true,
            });
        }

        [HttpPut("{id}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] EventDto eventDto)
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
