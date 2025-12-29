using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IAnimalService
    {
        public void Ajouter(Animal animal, string[] couleurs, int contactId, RaisonEntree raison, DateTime dateEntree);
        public Animal Consulter(string id);
        public void Supprimer(string id);
        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null);
    }
}
