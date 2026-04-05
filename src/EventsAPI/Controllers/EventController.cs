using EventsAPI.DTOs;
using EventsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EventsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EventController : ControllerBase
    {
        private IEventService _eventService;

        public EventController(IEventService eventService) 
        { 
            _eventService = eventService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            //_eventService
            return Ok();
        }

        [HttpGet("/{id}")]
        public IActionResult GetById([FromRoute] int id)
        {
            return Ok();
        }

        [HttpPost()]
        public IActionResult Create([FromBody] EventDto eventDto)
        {
            return Ok();
        }

        [HttpPut("/{id}")]
        public IActionResult Update([FromRoute] int id, [FromBody] EventDto eventDto)
        {
            return Ok();
        }

        [HttpDelete("/{id}")]
        public IActionResult Delete([FromRoute] int id)
        {
            return Ok();
        }

    }
}
