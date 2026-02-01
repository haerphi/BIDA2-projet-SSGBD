using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IContactService
    {
        public Contact Consulter(int id, bool includeRole = false);
        public IEnumerable<Contact> Lister(ContactFilters? filters = null, bool includeRole = false);
        public void Ajouter(Contact contact);
        public IEnumerable<Role> ListerRoles();
        public void MettreAJour(Contact contact);
        public IEnumerable<Adoption> ListerAdoptions(int contactId, bool includeAnimal = false, int offset = 0, int limit = 20);
    }
}
