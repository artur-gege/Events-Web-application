using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ModsenAPI.Models;
using ModsenAPI.Services.Interfaces;
using ModsenAPI.ModelsDTO;
using ModsenAPI.Validators;
using FluentValidation;

namespace ModsenAPI.Controllers
{
    [ApiController]
    [Route("api")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantService _participantService;
        private readonly IMapper _mapper;
        private readonly IValidator<ParticipantDto> _participantValidator;

        public ParticipantController(IParticipantService participantService, IMapper mapper, IValidator<ParticipantDto> participantValidator)
        {
            _participantService = participantService;
            _mapper = mapper;
            _participantValidator = participantValidator;
        }

        [HttpGet]
        [Route("events/{id}/participants")]
        public async Task<IActionResult> GetParticipantsByEventIdAsync(int id)
        {
            var participants = await _participantService.GetParticipantsByEventIdAsync(id);

            var participantsDto = _mapper.Map<List<ParticipantDto>>(participants);

            return Ok(participantsDto);
        }

        [HttpGet]
        [Route("participants/{id}")]
        public async Task<IActionResult> GetParticipantByIdAsync(int id)
        {
            var participant = await _participantService.GetParticipantByIdAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            var participantDto = _mapper.Map<ParticipantDto>(participant);

            return Ok(participantDto);
        }

        [HttpPost]
        [Route("events/{id}/participants")]
        public async Task<IActionResult> AddParticipantAsync(int id, [FromBody] ParticipantDto newParticipantDto)
        {
            var validationResult = _participantValidator.Validate(newParticipantDto);

            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors);
            }

            var newParticipant = _mapper.Map<Participant>(newParticipantDto);
            newParticipant.EventId = id;

            var addedParticipant = await _participantService.AddParticipantAsync(newParticipant);

            var addedParticipantDto = _mapper.Map<ParticipantDto>(addedParticipant);

            return CreatedAtAction(nameof(GetParticipantByIdAsync), new { id = addedParticipant.Id }, addedParticipantDto);
        }

        [HttpDelete]
        [Route("(participants/{id}")]
        public async Task<IActionResult> DeleteParticipantAsync(int id)
        {
            var participant = await _participantService.GetParticipantByIdAsync(id);

            if (participant == null)
            {
                return NotFound();
            }

            await _participantService.DeleteParticipantAsync(id);

            return NoContent();
        }

    }
}
