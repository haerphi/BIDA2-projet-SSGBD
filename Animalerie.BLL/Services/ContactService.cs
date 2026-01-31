using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.BLL.Services
{
    public class ContactService : IContactService
    {
        private readonly IContactRepository _contactRepository;

        public ContactService(IContactRepository contactRepository)
        {
            _contactRepository = contactRepository;
        }

        public Contact Consulter(int id, bool includeRole = false)
        {
            Contact? contact = _contactRepository.Consulter(id, includeRole);
            if (contact == null)
            {
                // TODO Custom Exception
                throw new Exception($"Le contact avec l'identifiant {id} n'a pas été trouvé.");
            }

            return contact;
        }

        public IEnumerable<Contact> Lister(ContactFilters? filters = null, bool includeRole = false)
        {
            return _contactRepository.Lister(filters, includeRole);
        }

        public void Ajouter(Contact contact)
        {
            // vérifier si le contact avec le même RegistreNational existe déjà
            var existingContact = _contactRepository.Lister(new ContactFilters { RegistreNational = contact.RegistreNational });
            if (existingContact.Any())
            {
                throw new Exception($"Un contact avec le Registre National {contact.RegistreNational} existe déjà.");
            }

            // vérifier si le contact avec le même email existe déjà

            if (contact.Email is not null)
            {
                existingContact = _contactRepository.Lister(new ContactFilters { Email = contact.Email });
                if (existingContact.Any())
                {
                    throw new Exception($"Un contact avec l'Email {contact.Email} existe déjà.");
                }
            }

            _contactRepository.Ajouter(contact);
        }
    }
}
