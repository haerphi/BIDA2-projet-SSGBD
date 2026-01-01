using Animalerie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.DAL.Mappers
{
    internal static class FamilleAcceuilMappers
    {
        internal static FamilleAccueil ToFamilleAccueil(this IDataRecord record)
        {
            return new FamilleAccueil
            (
                (int)record["id"],
                (DateTime)record["date_debut"],
                record["date_fin"] == DBNull.Value ? (DateTime?)null : (DateTime)record["date_fin"],
                (string)record["ani_id"],
                (int)record["famille_accueil_id"]
            );
        }
    }
}
