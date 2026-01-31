using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using System.Data;

namespace Animalerie.DAL.Mappers
{
    internal static class ContactMappers
    {
        internal static Contact ToContact(this IDataRecord record)
        {
            return new Contact(
                (int)record["id"],
                (string)record["nom"],
                (string)record["prenom"],
                record["rue"] == DBNull.Value ? null : (string)record["rue"],
                record["cp"] == DBNull.Value ? null : (string)record["cp"],
                record["localite"] == DBNull.Value ? null : (string)record["localite"],
                (string)record["registre_national"],
                record["gsm"] == DBNull.Value ? null : (string)record["gsm"],
                record["telephone"] == DBNull.Value ? null : (string)record["telephone"],
                record["email"] == DBNull.Value ? null : (string)record["email"]
            );
        }

        internal static PersonneRole ToPersonneRole(this IDataRecord record)
        {
            return new PersonneRole
            {
                RolId = (int)record["role_id"],
                Nom = (RoleNom)record["nom"]
            };
        }

        internal static Role ToRole(this IDataRecord record)
        {
            return new Role
            {
                Id = (int)record["id"],
                Nom = (RoleNom)record["nom"]
            };
        }
    }
}
