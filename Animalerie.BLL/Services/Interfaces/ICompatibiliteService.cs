using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface ICompatibiliteService
    {
        IEnumerable<Compatibilite> Lister();
        Compatibilite Consulter(int id);
        public void Modifier(Compatibilite compatibilite);
        public Compatibilite Ajouter(Compatibilite compatibilite);
    }
}
