using Animalerie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.DAL.Mappers
{
    internal static class CompatibiliteMappers
    {
        internal static Compatibilite ToCompatibilite(this IDataRecord record)
        {
            return new Compatibilite(
                (int)record["id"],
                (string)record["type"]
            );
        }
    }
}
