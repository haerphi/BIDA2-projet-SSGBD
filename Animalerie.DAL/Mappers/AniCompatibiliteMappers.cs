using Animalerie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.DAL.Mappers
{
    internal static class AniCompatibiliteMappers
    {
        internal static AniCompatibilite ToAniCompatibilite(this IDataRecord record)
        {
            return new AniCompatibilite(
                compType: record.GetString(record.GetOrdinal("type")),
                valeur: record.GetBoolean(record.GetOrdinal("valeur")),
                description: record.GetString(record.GetOrdinal("description")),
                compId: record.GetInt32(record.GetOrdinal("comp_id")),
                aniId: record.GetString(record.GetOrdinal("ani_id")),
                updatedAt: record.GetDateTime(record.GetOrdinal("updated_at"))
            );
        }
    }
}
