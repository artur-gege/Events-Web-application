
namespace ModsenAPI.Models
{
    public class Event
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public DateTime DateTime { get; set; }
        public string? Location { get; set; }
        public string? Category { get; set; }
        public int MaxParticipants { get; set; }
        public List<Participant> Participants { get; set; } = new List<Participant>();
        public string? ImageUrl { get; set; }
    }
}
