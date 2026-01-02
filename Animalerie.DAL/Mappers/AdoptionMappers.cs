using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using System.Data;

namespace Animalerie.DAL.Mappers
{
    internal static class AdoptionMappers
    {
        public static Adoption ToAdoption(this IDataRecord record)
        {
            Adoption adoption = new Adoption(
                (int)record["id"],
                (StatutAdoption)record["statut"],
                (DateTime)record["date_demande"],
                record["note"] == DBNull.Value ? null : (string?)record["note"],
                (string)record["ani_id"],
                (int)record["adoptant_id"]
            );

            return adoption;
        }
    }
}
