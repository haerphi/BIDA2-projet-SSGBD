using Animalerie.Domain.Models;
using System.Data;

namespace Animalerie.DAL.Mappers
{
    internal static class VaccinMappers
    {
        public static Vaccin ToVaccin(this IDataRecord record)
        {
            return new Vaccin
            {
                Id = (int)record["id"],
                Nom = (string)record["nom"]
            };
        }

        public static Vaccination ToVaccination(this IDataRecord record)
        {
            return new Vaccination
            {
                Id = (int)record["va_id"],
                Date = (DateTime)record["date"],
                AniId = (string)record["ani_id"],
                VacId = (int)record["vac_id"],
                Vaccin = new Vaccin
                {
                    Id = (int)record["vac_id"],
                    Nom = (string)record["nom"]
                }
            };
        }
    }
}
