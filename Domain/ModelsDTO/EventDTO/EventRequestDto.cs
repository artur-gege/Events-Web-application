namespace ModsenAPI.Domain.ModelsDTO.EventDTO
{
    public class EventRequestDto
    {
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
        public int MaxParticipants { get; set; }
        public string? ImageUrl { get; set; }
    }
}
