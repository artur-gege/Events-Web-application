using ModsenAPI.Domain.Models;

namespace ModsenAPI.Domain.ModelsDTO.EventDTO
{
    public class EventResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
        public int MaxParticipants { get; set; }
        public string? ImageUrl { get; set; }

        public EventResponseDto(Event eventEntity)
        {
            Id = eventEntity.Id;
            Name = eventEntity.Name;
            Description = eventEntity.Description;
            DateTime = eventEntity.DateTime;
            Location = eventEntity.Location;
            Category = eventEntity.Category;
            MaxParticipants = eventEntity.MaxParticipants;
            ImageUrl = eventEntity.ImageUrl;
        }
        public EventResponseDto()
        {

        }
    }
}
