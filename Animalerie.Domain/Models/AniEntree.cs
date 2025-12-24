using Animalerie.Domain.CustomEnums.Database;

namespace Animalerie.Domain.Models
{
    public class AniEntree
    {
        public int Id { get; set; }
        public RaisonEntree Raison { get; set; }
        public DateTime Date { get; set; }
        public string AniId { get; set; }
        public int ContactId { get; set; }
    }
}
