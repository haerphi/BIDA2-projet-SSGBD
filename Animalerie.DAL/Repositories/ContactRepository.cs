using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
using System.Data.Common;
using Tools.Database;

namespace Animalerie.DAL.Repositories
{
    public class ContactRepository : IContactRepository
    {
        DbConnection _connection;

        public ContactRepository(AnimalerieDBContext dbContext)
        {
            _connection = dbContext.Connection;
        }

        public Contact? Consulter(int id)
        {
            return _connection.ExecuteReader<Contact>(
                "SELECT id, nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email FROM CONTACT WHERE id = @id",
                r => r.ToContact(),
                false,
                new { id }
            ).FirstOrDefault();
        }

        public IEnumerable<Contact> Lister()
        {
            return _connection.ExecuteReader<Contact>(
                "SELECT id, nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email FROM CONTACT",
                r => r.ToContact(),
                false
            );
        }

        public IEnumerable<Contact> ListerParIds(IEnumerable<int> ids)
        {
            if (ids.Any())
            {
                return _connection.ExecuteReader<Contact>(
                    $"SELECT id, nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email FROM CONTACT WHERE id IN ({string.Join(",", ids)})",
                    r => r.ToContact(),
                    false
                );
            }
            else
            {
                return Enumerable.Empty<Contact>();
            }
        }
    }
}
