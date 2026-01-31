using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IContactRepository
    {
        public Contact? Consulter(int id, bool includeRole = false);
        public IEnumerable<Contact> Lister(ContactFilters? filters = null, bool includeRole = false);
        public IEnumerable<Contact> ListerParIds(IEnumerable<int> ids, bool includeRole = false);
        public void Ajouter(Contact contact);
    }
}
