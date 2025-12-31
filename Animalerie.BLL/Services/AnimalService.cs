using Animalerie.BLL.CustomExceptions;
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

        public AnimalService(IAnimalRepository animalRepository, IContactService contactService)
        {
            _animalRepository = animalRepository;
            _contactService = contactService;
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
            throw new NotImplementedException();
        }

        public IEnumerable<AniCompatibilite> ListCompatibilites(string animalId)
        {
            return _animalRepository.ListCompatibilites(animalId);
        }

        public void Supprimer(string id)
        {
            throw new NotImplementedException();
        }
    }
}
