using Animalerie.Domain.CustomEnums.Database;

namespace Animalerie.Domain.Models
{
    public class AniSortie
    {
        public int Id { get; set; }
        public RaisonSortie Raison { get; set; }
        public DateTime Date { get; set; }
        public string AniId { get; set; }
        public int ContactId { get; set; }
    }
}
