using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IAnimalRepository
    {
        public void AjouterAnimal(Animal animal, string[] couleurs, Contact contact, RaisonEntree raison, DateTime dateEntree);
        public Animal? ConsulterAnimal(string id);
        public void SupprimerAnimal(string id);
        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null);
    }
}
