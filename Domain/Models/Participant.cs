using System.ComponentModel.DataAnnotations.Schema;

namespace ModsenAPI.Domain.Models
{
    // убрал data annotations в доменной модели
    public class Participant
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime RegistrationDate { get; set; }
        public string? Email { get; set; }
        public int EventId { get; set; }
        public Event? Event { get; set; }
    }
}