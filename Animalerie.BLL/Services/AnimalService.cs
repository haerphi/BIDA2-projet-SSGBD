using Animalerie.BLL.CustomExceptions;
using Animalerie.BLL.CustomExceptions.Animal;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.BLL.Services
{
    public class AnimalService : IAnimalService
    {
        private readonly IAnimalRepository _animalRepository;
        private readonly IContactService _contactService;
        private readonly ICompatibiliteService _compatibiliteService;

        public AnimalService(IAnimalRepository animalRepository, IContactService contactService, ICompatibiliteService compatibiliteService)
        {
            _animalRepository = animalRepository;
            _contactService = contactService;
            _compatibiliteService = compatibiliteService;
        }

        public void Ajouter(Animal animal, string[] couleurs, int contactId, RaisonEntree raison, DateTime dateEntree)
        {
            // Récupération du contact associé
            Contact contact = _contactService.Consulter(contactId);

            // Ajout de l'animal via le repository
            _animalRepository.Ajouter(animal, couleurs, contact, raison, dateEntree);

            // TODO récupérer l'animal ajouté (ou au moins son ID)
        }

        public Animal Consulter(string id)
        {
            Animal? animal = _animalRepository.Consulter(id);
            if (animal == null)
            {
                throw new NotFoundException();
            }
            return animal;
        }

        public IEnumerable<Animal> Lister(AnimalFilters? filters = null, int offset = 0, int limit = 20)
        {
            return _animalRepository.Lister(filters, offset, limit);
        }

        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null)
        {
            Compatibilite compatibilite = _compatibiliteService.Consulter(compId);
            AniCompatibilite aniCompatibilite = new AniCompatibilite(compatibilite, aniId, valeur, desc, DateTime.Now);

            _animalRepository.ModifierCompatibilite(aniCompatibilite);
        }

        public IEnumerable<AniCompatibilite> ListCompatibilites(string animalId)
        {
            return _animalRepository.ListCompatibilites(animalId);
        }

        public void Supprimer(string id)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<FamilleAccueil> ListerFamillesAccueil(string animalId, bool includeContact = false, int offset = 0, int limit = 20)
        {
            return _animalRepository.ListerFamillesAccueil(animalId, includeContact, offset, limit);
        }

        public FamilleAccueil ConsulterFamilelAccueil(int familleid)
        {
            FamilleAccueil? fa = _animalRepository.ConsulterFamilelAccueil(familleid);
            if (fa is null)
            {
                throw new NotFoundException();
            }
            return fa;
        }

        public FamilleAccueil? FamilleAccueilActuelle(string animalId, bool includeContact)
        {
            return _animalRepository.FamilleAccueilActuelle(animalId, includeContact);
        }

        public void MettreEnFamilleAccueil(string animalId, int contactId, DateTime dateDebut, DateTime? dateFin = null)
        {
            // vérification si l'animal et le contact existent
            Animal animal = Consulter(animalId);
            Contact contact = _contactService.Consulter(contactId);

            FamilleAccueil familleAccueil = new FamilleAccueil(-1, dateDebut, dateFin, animalId, contactId);
            familleAccueil.Animal = animal;
            familleAccueil.Contact = contact;

            try
            {
                _animalRepository.MettreEnFamilleAccueil(familleAccueil);
            }
            catch (Npgsql.PostgresException ex)
            {
                if (ex.MessageText == "Cet animal est dejà dans une famille d'accueil.")
                {
                    throw new DejaEnFamilleAcceuilException();
                }
            }
        }

        public void ModifierFamilleAccueil(FamilleAccueil familleAccueil)
        {
            FamilleAccueil fa = ConsulterFamilelAccueil(familleAccueil.Id);
            
            fa.ContactId = familleAccueil.ContactId;
            fa.DateDebut = familleAccueil.DateDebut;
            fa.DateFin = familleAccueil.DateFin;

            _animalRepository.ModifierFamilleAccueil(fa);
        }

        public IEnumerable<Adoption> ListerAdoptions(string animalId, bool includeContact = false, int offset = 0, int limit = 20)
        {
            return _animalRepository.ListerAdoptions(animalId, includeContact, offset, limit);
        }
    }
}
