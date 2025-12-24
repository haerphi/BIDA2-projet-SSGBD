namespace Animalerie.Domain.Models
{
    public class Vaccination
    {
        public int Id { get; set; }
        public DateTime? Date { get; set; }
        public string AniId { get; set; }
        public int VacId { get; set; }
    }
}
