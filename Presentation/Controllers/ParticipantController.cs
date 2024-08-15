using Microsoft.AspNetCore.Mvc;
using ModsenAPI.Application.UseCases.Interfaces;
using ModsenAPI.Domain.ModelsDTO.ParticipantDTO;

namespace ModsenAPI.Presentation.Controllers
{
    [ApiController]
    [Route("api")]
    public class ParticipantController : ControllerBase
    {
        private readonly IParticipantUseCase _participantUseCase;

        public ParticipantController(IParticipantUseCase participantUseCase)
        {
            _participantUseCase = participantUseCase;
        }

        [HttpGet]
        [Route("events/{id}/participants")]
        public async Task<IActionResult> GetParticipantsByEventIdAsync(int id)
        {
            var participants = await _participantUseCase.GetParticipantsByEventIdAsync(id);
            return Ok(participants);
        }

        [HttpGet]
        [Route("participants/{id}")]
        public async Task<IActionResult> GetParticipantByIdAsync(int id)
        {
            var participant = await _participantUseCase.GetParticipantByIdAsync(id);
            return Ok(participant);
        }

        [HttpPost]
        [Route("events/{id}/participants")]
        public async Task<IActionResult> AddParticipantAsync(int id, [FromBody] ParticipantRequestDto newParticipant)
        {
            var participant = await _participantUseCase.AddParticipantAsync(id, newParticipant);
            return CreatedAtAction(nameof(GetParticipantByIdAsync), new { id = participant.Id }, participant);
        }

        [HttpDelete]
        [Route("(participants/{id}")]
        public async Task<IActionResult> DeleteParticipantAsync(int id)
        {
            await _participantUseCase.DeleteParticipantAsync(id);
            return NoContent();
        }

    }
}
