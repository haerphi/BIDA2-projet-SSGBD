using Animalerie.Domain.Models;
using Animalerie.WPF.Models.Compatibilites;

namespace Animalerie.WPF.Models.Animals
{
    internal class AnimalCompatibiliteModel
    {
        public CompatibiliteModel Compatibilite { get; set; }
        public string AniId { get; set; }
        public bool Valeur { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
