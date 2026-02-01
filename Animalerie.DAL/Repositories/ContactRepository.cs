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

            if (filters != null)
            {
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
                "SELECT * FROM fn_lister_roles_contact_table(@p_contactid)",
                r => r.ToPersonneRole(),
                false,
                new { p_contactid = contactId }
            );
        }

        public void Ajouter(Contact contact)
        {
            object? result = _connection.ExecuteScalar("SELECT ps_ajouter_contact(@p_nom, @p_prenom, @p_registre_national, @p_rue, @p_cp, @p_localite, @p_gsm, @p_telephone, @p_email)", false, new
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

            int id = Convert.ToInt32(result);


            foreach (var role in contact.Roles)
            {
                AjouterRoleContact(id, role.RolId);
            }
        }

        public IEnumerable<Role> ListerRoles()
        {
            return _connection.ExecuteReader<Role>(
                "SELECT id, nom FROM ROLE",
                r => r.ToRole(),
                false
            ).ToList();
        }

        public void MettreAJour(Contact contact)
        {
            _connection.ExecuteNonQuery("ps_modifier_contact", true, new
            {
                p_contact_id = contact.Id,
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
        }

        public void AjouterRoleContact(int contactId, int roleId)
        {
            _connection.ExecuteNonQuery("ps_ajouter_role_contact", true, new
            {
                p_contact_id = contactId,
                p_role_id = roleId
            });
        }

        public void RetirerRoleContact(int contactId, int roleId)
        {
            _connection.ExecuteNonQuery("ps_retirer_role_contact", true, new
            {
                p_contact_id = contactId,
                p_role_id = roleId
            });
        }

        public IEnumerable<Adoption> ListerAdoptions(int contactId, int offset = 0, int limit = 20)
        {
            List<Adoption> adoptions = _connection.ExecuteReader<Adoption>(
                "SELECT * FROM adoption WHERE adoptant_id = @p_adoptant_id ORDER BY date_demande DESC LIMIT @p_limit OFFSET @p_offset",
                (r) => r.ToAdoption(),
                false,
                new
                {
                    p_adoptant_id = contactId,
                    p_limit = limit,
                    p_offset = offset
                }
            ).ToList();
            return adoptions;
        }
    }
}
