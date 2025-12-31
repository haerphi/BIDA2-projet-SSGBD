using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IAnimalRepository
    {
        public void Ajouter(Animal animal, string[] couleurs, Contact contact, RaisonEntree raison, DateTime dateEntree);
        public Animal? Consulter(string id);
        public IEnumerable<Animal> Lister(AnimalFilters? filters = null, int offset = 0, int limit = 20);
        public void Supprimer(string id);
        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null);
        public IEnumerable<AniCompatibilite> ListCompatibilites(string animalId);
    }
}
