using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IAdoptionService
    {
        public Adoption Consulter(int adoptionId, bool includeContact = false, bool includeAnimal = false);
        public void Ajouter(string animalId, int contactId, string? note = null, StatutAdoption statut = StatutAdoption.Demande);
        public void Modifier(int adoptionId, StatutAdoption statut, string? note);
    }
}
