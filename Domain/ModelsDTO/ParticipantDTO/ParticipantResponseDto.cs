namespace ModsenAPI.Domain.ModelsDTO.ParticipantDTO
{
    public class ParticipantResponseDto
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? Email { get; set; }
        public int EventId { get; set; }
    }
}
