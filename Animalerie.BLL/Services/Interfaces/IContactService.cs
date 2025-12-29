using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IContactService
    {
        public Contact Consulter(int id);
        public IEnumerable<Contact> Lister();
    }
}
