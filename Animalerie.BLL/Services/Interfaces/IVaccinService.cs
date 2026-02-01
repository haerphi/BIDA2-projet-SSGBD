using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services.Interfaces
{
    public interface IVaccinService
    {
        public IEnumerable<Vaccin> Lister();
        public Vaccin Consulter(int id);
        public int Ajouter(Vaccin vaccin);
        public void MettreAJour(Vaccin vaccin);
    }
}
