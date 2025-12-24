using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IAnimalService
    {
        public void Ajouter(Animal animal, string[] couleurs, int contactId);
        public Animal Consulter(string id);
        public void Supprimer(string id);
        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null);
    }
}
