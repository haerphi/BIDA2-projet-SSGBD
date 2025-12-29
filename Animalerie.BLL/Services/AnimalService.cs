using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;

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
            _animalRepository.AjouterAnimal(animal, couleurs, contact, raison, dateEntree);

            // TODO récupérer l'animal ajouté (ou au moins son ID)
        }

        public Animal Consulter(string id)
        {
            throw new NotImplementedException();
        }

        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null)
        {
            throw new NotImplementedException();
        }

        public void Supprimer(string id)
        {
            throw new NotImplementedException();
        }
    }
}
