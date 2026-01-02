using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IAdoptionRepository
    {
        public Adoption? Consulter(int adoptionId, bool includeContact = false, bool includeAnimal = false);
        public void Ajouter(Adoption adoption = null);
        public void Modifier(Adoption adoption);
    }
}
