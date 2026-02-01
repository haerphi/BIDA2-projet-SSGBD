using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IContactRepository
    {
        public Contact? Consulter(int id, bool includeRole = false);
        public IEnumerable<Contact> Lister(ContactFilters? filters = null, bool includeRole = false);
        public IEnumerable<Contact> ListerParIds(IEnumerable<int> ids, bool includeRole = false);
        public IEnumerable<PersonneRole> ListerRoleContact(int contactId);
        public void Ajouter(Contact contact);
        public IEnumerable<Role> ListerRoles();
        public void MettreAJour(Contact contact);
        public void AjouterRoleContact(int contactId, int roleId);
        public void RetirerRoleContact(int contactId, int roleId);
        public IEnumerable<Adoption> ListerAdoptions(int contactId, int offset = 0, int limit = 20);
    }
}
