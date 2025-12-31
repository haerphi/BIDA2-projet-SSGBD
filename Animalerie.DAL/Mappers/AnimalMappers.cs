using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.DAL.Mappers
{
    internal static class AnimalMappers
    {
        internal static Animal ToAnimal(this IDataRecord record)
        {
            Animal a =  new Animal(
                (string)record["id"],
                (string)record["nom"],
                (TypeAnimal)record["type"],
                (SexeAnimal)record["sexe"],
                record["particularites"] == DBNull.Value ? "" : (string)record["particularites"],
                record["description"] == DBNull.Value ? "" : (string)record["description"],
                record["date_sterilisation"] == DBNull.Value ? null : (DateTime?)record["date_sterilisation"],
                (DateTime)record["date_naissance"],
               Array.Empty<string>(), // TODO Récupérer les couleurs
                record["date_deces"] == DBNull.Value ? null : (DateTime?)record["date_deces"],
                record["deleted_at"] == DBNull.Value ? null : (DateTime?)record["deleted_at"]
            );

            a.Status = record["status"] == DBNull.Value ? null : (string)record["status"];

            return a;
        } 
    }
}
