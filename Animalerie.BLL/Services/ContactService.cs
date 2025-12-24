using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services
{
    public class ContactService: IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Contact Consulter(int id)
        {
            Contact? contact = _contactRepository.Consulter(id);
            if (contact == null)
            {
                // TODO Custom Exception
                throw new Exception($"Le contact avec l'identifiant {id} n'a pas été trouvé.");
            }

            return contact;
        }
    }
}
