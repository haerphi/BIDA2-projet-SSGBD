using Animalerie.BLL.Services.Interfaces;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;

namespace Animalerie.BLL.Services
{
    public class VaccinService : IVaccinService
    {
        private readonly IVaccinRepository _vaccinRepository;

        public VaccinService(IVaccinRepository vaccinRepository)
        {
            _vaccinRepository = vaccinRepository;
        }

        public int Ajouter(Vaccin vaccin)
        {
            return _vaccinRepository.Ajouter(vaccin);
        }

        public Vaccin Consulter(int id)
        {
            Vaccin? vaccin = _vaccinRepository.Consulter(id);
            if (vaccin == null)
            {
                throw new Exception($"Le vaccin avec l'identifiant {id} n'a pas été trouvé.");
            }

            return vaccin;
        }

        public IEnumerable<Vaccin> Lister()
        {
            return _vaccinRepository.Lister();
        }

        public void MettreAJour(Vaccin vaccin)
        {
            _vaccinRepository.MettreAJour(vaccin);
        }
    }
}
