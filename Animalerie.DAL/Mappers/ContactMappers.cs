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
    }
}
