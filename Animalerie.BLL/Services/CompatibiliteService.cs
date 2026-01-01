using Animalerie.BLL.CustomExceptions;
using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services
{
    public class CompatibiliteService : ICompatibiliteService
    {
        ICompatibiliteRepository _compatibiliteRepository;

        public CompatibiliteService(ICompatibiliteRepository compatibiliteRepository)
        {
            _compatibiliteRepository = compatibiliteRepository;
        }

        public IEnumerable<Compatibilite> Lister()
        {
            return _compatibiliteRepository.Lister();
        }

        public Compatibilite Consulter(int id)
        {
            var compatibilite = _compatibiliteRepository.Consulter(id);
            if (compatibilite == null)
            {
                throw new NotFoundException();
            }
            return compatibilite;
        }
    }
}
