using Animalerie.BLL.CustomExceptions;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services
{
    public class AdoptionService : IAdoptionService
    {
        private readonly IAdoptionRepository _adoptionRepository;
        private readonly IContactService _contactService;
        private readonly IAnimalService _animalService;

        public AdoptionService(IAdoptionRepository adoptionRepository, IContactService contactService, IAnimalService animalService)
        {
            _adoptionRepository = adoptionRepository;
            _contactService = contactService;
            _animalService = animalService;
        }

        public Adoption Consulter(int adoptionId, bool includeContact = false, bool includeAnimal = false)
        {
            Adoption? adoption = _adoptionRepository.Consulter(adoptionId, includeContact, includeAnimal);

            if (adoption == null)
            {
                throw new NotFoundException();
            }
            return adoption;
        }

        public void Ajouter(string animalId, int contactId, string? note = null)
        {
            Animal animal = _animalService.Consulter(animalId);
            Contact contact = _contactService.Consulter(contactId);
            Adoption adoption = new Adoption(animalId, contactId, note);
            _adoptionRepository.Ajouter(adoption);
        }

        public void Modifier(int adoptionId, StatutAdoption statut, string? note)
        {
            Adoption adoption = Consulter(adoptionId);
            adoption.Statut = statut;
            adoption.Note = note;
            _adoptionRepository.Modifier(adoption);
        }
    }
}
