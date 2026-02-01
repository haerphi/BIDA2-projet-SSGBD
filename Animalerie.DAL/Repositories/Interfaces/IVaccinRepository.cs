using Animalerie.Domain.Models;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface IVaccinRepository
    {
        public IEnumerable<Vaccin> Lister();
        public Vaccin? Consulter(int id);
        public int Ajouter(Vaccin vaccin);
        public void MettreAJour(Vaccin vaccin);
    }
}
