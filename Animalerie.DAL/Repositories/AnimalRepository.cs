using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;
using System.Data.Common;
using Tools.Database;

namespace Animalerie.DAL.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        AnimalerieDBContext _dbContext;

        public AnimalRepository(AnimalerieDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void Ajouter(Animal animal, string[] couleurs, Contact contact, RaisonEntree raison, DateTime dateEntree)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_ajouter_animal", true, new
            {
                p_contact_id = contact.Id,
                p_id = animal.Id,
                p_nom = animal.Nom,
                p_type = animal.Type,
                p_sexe = animal.Sexe,
                p_date_naissance = animal.DateNaissance,
                p_couleurs = couleurs,
                p_particularites = animal.Particularites,
                p_description = animal.Description,
                p_date_sterilisation = animal.DateSterilisation,
                p_raison = raison,
                p_entree_date = dateEntree
            });
        }

        public Animal? Consulter(string id)
        {
            return _dbContext.Connection.ExecuteReader<Animal>(
                "SELECT * FROM vue_animaux WHERE id = @p_id AND deleted_at IS NULL",
                (r) => r.ToAnimal(),
                false,
                new { p_id = id }
            ).FirstOrDefault();
        }

        public IEnumerable<Animal> Lister(AnimalFilters? filters = null, int offset = 0, int limit = 20)
        {
            string query = "SELECT * FROM vue_animaux WHERE deleted_at IS NULL";

            if (filters is not null)
            {
                if (filters.Nom is not null)
                {
                    query += " AND nom LIKE '%' || @p_nom || '%'";
                }

                if (filters.Type is not null)
                {
                    query += " AND type = @p_type";
                }

                if (filters.Sexe is not null)
                {
                    query += " AND sexe = @p_sexe";
                }

                if (filters.AnimalStatus is not null)
                {
                    query += " AND status LIKE @p_statut || '%'";
                }
            }

            query += " ORDER BY id LIMIT @p_limit OFFSET @p_offset";

            return _dbContext.Connection.ExecuteReader<Animal>(query, (r) => r.ToAnimal(), false, new
            {
                p_nom = filters?.Nom,
                p_type = filters?.Type,
                p_sexe = filters?.Sexe,
                p_statut = filters?.AnimalStatus.ToString(),

                p_limit = limit,
                p_offset = offset
            });
        }

        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<AniCompatibilite> ListCompatibilites(string animalId)
        {
            return _dbContext.Connection.ExecuteReader<AniCompatibilite>(
                "SELECT * FROM vue_animaux_compatibilites WHERE ani_id = @p_ani_id",
                (r) => r.ToAniCompatibilite(),
                false,
                new { p_ani_id = animalId }
            );
        }

        public void Supprimer(string id)
        {
            throw new NotImplementedException();
        }
    }
}
