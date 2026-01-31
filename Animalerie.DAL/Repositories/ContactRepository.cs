using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;
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

        public Contact? Consulter(int id, bool includeRole = false)
        {
            Contact? c = _connection.ExecuteReader<Contact>(
                "SELECT id, nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email FROM CONTACT WHERE id = @id",
                r => r.ToContact(),
                false,
                new { id }
            ).FirstOrDefault();

            if (c is not null)
            {
                if (includeRole)
                {
                    c.Roles = ListerRoleContact(c.Id).ToList();
                }
            }

            return c;
        }

        public IEnumerable<Contact> Lister(ContactFilters? filters = null, bool includeRole = false)
        {
            string query = "SELECT id, nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email FROM CONTACT";

            if (filters != null) {
                List<string> conditions = [];
                if (!string.IsNullOrEmpty(filters.Firstname))
                {
                    conditions.Add("prenom ILIKE '%' || @firstname || '%'");
                }
                if (!string.IsNullOrEmpty(filters.Lastname))
                {
                    conditions.Add("nom ILIKE '%' || @lastname || '%'");
                }
                if (!string.IsNullOrEmpty(filters.RegistreNational))
                {
                    conditions.Add("registre_national = @registernational");
                }
                if (!string.IsNullOrEmpty(filters.Email))
                {
                    conditions.Add("email ILIKE '%' || @email || '%'");
                }

                if (conditions.Any())
                {
                    query += " WHERE " + string.Join(" AND ", conditions);
                }
            }

            IEnumerable<Contact> contacts = _connection.ExecuteReader<Contact>(
                query,
                r => r.ToContact(),
                false,
                new
                {
                    firstname = filters?.Firstname,
                    lastname = filters?.Lastname,
                    registernational = filters?.RegistreNational,
                    email = filters?.Email
                }
            ).ToList();

            if (includeRole)
            {
                foreach (var contact in contacts)
                {
                    contact.Roles = ListerRoleContact(contact.Id).ToList();
                }
            }

            return contacts;
        }

        public IEnumerable<Contact> ListerParIds(IEnumerable<int> ids, bool includeRole = false)
        {
            IEnumerable<Contact> contacts = Enumerable.Empty<Contact>();
            if (ids.Any())
            {
                contacts = _connection.ExecuteReader<Contact>(
                    $"SELECT id, nom, prenom, rue, cp, localite, registre_national, gsm, telephone, email FROM CONTACT WHERE id IN ({string.Join(",", ids)})",
                    r => r.ToContact(),
                    false
                ).ToList();
                if (includeRole)
                {
                    foreach (var contact in contacts)
                    {
                        contact.Roles = ListerRoleContact(contact.Id).ToList();
                    }
                }
            }

            return contacts;
        }

        public IEnumerable<PersonneRole> ListerRoleContact(int contactId)
        {
            return _connection.ExecuteReader<PersonneRole>(
                "SELECT fn_lister_roles_contact_table(@p_contactid)",
                r => r.ToPersonneRole(),
                false,
                new { p_contactid = contactId }
            );
        }

        public void Ajouter(Contact contact)
        {
            _connection.ExecuteNonQuery("ps_ajouter_contact", true, new
            {
                p_nom = contact.Nom,
                p_prenom = contact.Prenom,
                p_registre_national = contact.RegistreNational,
                p_rue = contact.Rue,
                p_cp = contact.Cp,
                p_localite = contact.Localite,
                p_gsm = contact.Gsm,
                p_telephone = contact.Telephone,
                p_email = contact.Email
            });

            foreach (var role in contact.Roles)
            {
                _connection.ExecuteNonQuery("ps_ajouter_role_contact", true, new
                {
                    p_contactid = contact.Id,
                    p_roleid = role.RolId
                });
            }
        }
    }
}
