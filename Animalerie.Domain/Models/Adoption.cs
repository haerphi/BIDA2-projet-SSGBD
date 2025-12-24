using Animalerie.Domain.CustomEnums.Database;

namespace Animalerie.Domain.Models
{
    public class Adoption
    {
        public int Id { get; set; }
        public StatutAdoption Statut { get; set; }
        public DateTime DateDemande { get; set; }
        public string AniId { get; set; }
        public int AdoptantId { get; set; }
    }
}
