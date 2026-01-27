using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IAnimalService
    {
        public void Ajouter(Animal animal, string[] couleurs, int contactId, RaisonEntree raison, DateTime dateEntree);
        public IEnumerable<Animal> Lister(AnimalFilters? filters = null, int offset = 0, int limit = 20);
        public Animal Consulter(string id);
        public void Supprimer(string id);

        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null);
        public IEnumerable<AniCompatibilite> ListCompatibilites(string animalId);
        public IEnumerable<FamilleAccueil> ListerFamillesAccueil(string animalId, bool includeContact = false, int offset = 0, int limit = 20);
        public FamilleAccueil ConsulterFamilelAccueil(int familleid);
        public FamilleAccueil? FamilleAccueilActuelle(string animalId, bool includeContact);
        public void MettreEnFamilleAccueil(string animalId, int contactId, DateTime dateDebut, DateTime? dateFin = null);
        public void ModifierFamilleAccueil(FamilleAccueil familleAccueil);
        public IEnumerable<Adoption> ListerAdoptions(string animalId, bool includeContact = false, int offset = 0, int limit = 20);
    }
}
