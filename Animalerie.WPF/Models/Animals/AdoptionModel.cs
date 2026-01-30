using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.WPF.Models.Contacts;

namespace Animalerie.WPF.Models.Animals
{
    internal class AdoptionModel
    {
        public int Id { get; set; }
        public StatutAdoption Statut { get; set; }
        public DateTime DateDemande { get; set; }
        public string? Note { get; set; }
        public string AniId { get; set; }
        public int ContactId { get; set; }

        // Navigation properties
        public AnimalListingModel? Animal { get; set; }
        public ContactModel? Contact { get; set; }
    }
}
