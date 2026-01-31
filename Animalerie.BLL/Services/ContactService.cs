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
            contact.Email = contact.Email?.Trim();
            contact.Email = string.IsNullOrEmpty(contact.Email) ? null : contact.Email;
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

        public IEnumerable<Role> ListerRoles()
        {
            return _contactRepository.ListerRoles();
        }

        public void MettreAJour(Contact contact)
        {
            var existingContact = _contactRepository.Consulter(contact.Id, true);
            if (existingContact == null)
            {
                throw new Exception($"Le contact avec l'identifiant {contact.Id} n'a pas été trouvé.");
            }
            // vérifier si le contact avec le même RegistreNational existe déjà
            var contactsWithSameRN = _contactRepository.Lister(new ContactFilters { RegistreNational = contact.RegistreNational })
                .Where(c => c.Id != contact.Id);
            if (contactsWithSameRN.Any())
            {
                throw new Exception($"Un autre contact avec le Registre National {contact.RegistreNational} existe déjà.");
            }
            // vérifier si le contact avec le même email existe déjà
            if (contact.Email is not null)
            {
                var contactsWithSameEmail = _contactRepository.Lister(new ContactFilters { Email = contact.Email })
                    .Where(c => c.Id != contact.Id);
                if (contactsWithSameEmail.Any())
                {
                    throw new Exception($"Un autre contact avec l'Email {contact.Email} existe déjà.");
                }
            }
            _contactRepository.MettreAJour(contact);

            // Comparaison des role de "contact" et "existingContact" pour déterminer les ajouts et suppressions
            var rolesToAdd = contact.Roles.Where(r => !existingContact.Roles.Any(er => er.RolId == r.RolId)).ToList();
            var rolesToRemove = existingContact.Roles.Where(er => !contact.Roles.Any(r => r.RolId == er.RolId)).ToList();
            foreach (var role in rolesToAdd)
            {
                _contactRepository.AjouterRoleContact(contact.Id, role.RolId);
            }

            foreach (var role in rolesToRemove)
            {
                _contactRepository.RetirerRoleContact(contact.Id, role.RolId);
            }
        }
    }
}
