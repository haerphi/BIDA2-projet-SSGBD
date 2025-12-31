using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.CustomEnums.ListingFilters;

namespace Animalerie.Domain.Models.Listing
{
    public class AnimalFilters
    {
        public string? Nom { get; set; }
        public TypeAnimal? Type { get; set; }
        public SexeAnimal? Sexe { get; set; }
        public AnimalStatus? AnimalStatus { get; set; }
    }
}
