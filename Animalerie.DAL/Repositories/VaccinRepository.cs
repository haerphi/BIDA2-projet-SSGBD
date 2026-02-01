using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
using System.Data.Common;
using Tools.Database;

namespace Animalerie.DAL.Repositories
{
    public class VaccinRepository : IVaccinRepository
    {
        DbConnection _connection;

        public VaccinRepository(AnimalerieDBContext dbContext)
        {
            _connection = dbContext.Connection;
        }

        public int Ajouter(Vaccin vaccin)
        {
            object? result = _connection.ExecuteScalar("SELECT ps_ajouter_vaccin(@p_nom)", false, new
            {
                p_nom = vaccin.Nom,
            });

            int id = Convert.ToInt32(result);
            return id;
        }

        public Vaccin? Consulter(int id)
        {
            return _connection.ExecuteReader<Vaccin>("SELECT * FROM vaccin WHERE id = @p_id",
               v => v.ToVaccin(),
               false,
            new
            {
                p_id = id,
            }).FirstOrDefault();
        }

        public IEnumerable<Vaccin> Lister()
        {
            return _connection.ExecuteReader<Vaccin>("SELECT * FROM vaccin",
                v => v.ToVaccin(),
                false);
        }

        public void MettreAJour(Vaccin vaccin)
        {
            _connection.ExecuteNonQuery("ps_modifier_vaccin", true, new
            {
                p_vaccin_id = vaccin.Id,
                p_nom = vaccin.Nom,
            });
        }
    }
}
