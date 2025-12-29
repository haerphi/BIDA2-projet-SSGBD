using Animalerie.Domain.Models;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IContactRepository
    {
        public Contact? Consulter(int id);
        public IEnumerable<Contact> Lister();
    }
}
