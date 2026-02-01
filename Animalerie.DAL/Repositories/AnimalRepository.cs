using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.Models;
using Animalerie.Domain.Models.Listing;
using Tools.Database;

namespace Animalerie.DAL.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        AnimalerieDBContext _dbContext;
        IContactRepository _contactRepository;

        public AnimalRepository(AnimalerieDBContext dbContext, IContactRepository contactRepository)
        {
            _dbContext = dbContext;
            _contactRepository = contactRepository;
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

                if (filters.HasAdoptionRequest is not null)
                {
                    if (filters.HasAdoptionRequest.Value)
                    {
                        query += " AND EXISTS (SELECT 1 FROM adoption WHERE ani_id = vue_animaux.id)";
                    }
                    else
                    {
                        query += " AND NOT EXISTS (SELECT 1 FROM adoption WHERE ani_id = vue_animaux.id)";
                    }
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

        public void ModifierCompatibilite(AniCompatibilite aniCompatibilite)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_modifier_compatibilite_animal", true, new
            {
                p_ani_id = aniCompatibilite.AniId,
                p_comp_id = aniCompatibilite.Compatibilite.Id,
                p_valeur = aniCompatibilite.Valeur,
                p_description = aniCompatibilite.Description
            });
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

        public IEnumerable<FamilleAccueil> ListerFamillesAccueil(string animalId, bool includeContact = false, int offset = 0, int limit = 20)
        {
            List<FamilleAccueil> familleAccueils = _dbContext.Connection.ExecuteReader<FamilleAccueil>(
                "SELECT * FROM fn_lister_familles_accueil_animal(@p_animal_id) ORDER BY date_debut DESC LIMIT @p_limit OFFSET @p_offset",
                (r) => r.ToFamilleAccueil(),
                false,
                new
                {
                    p_animal_id = animalId,
                    p_limit = limit,
                    p_offset = offset
                }
            ).ToList();

            if (includeContact)
            {
                List<int> contactIds = familleAccueils.Select(fa => fa.ContactId).Distinct().ToList();
                List<Contact> contacts = _contactRepository.ListerParIds(contactIds).ToList();

                foreach (var familleAccueil in familleAccueils)
                {
                    familleAccueil.Contact = contacts.FirstOrDefault(c => c.Id == familleAccueil.ContactId);
                }
            }

            return familleAccueils;
        }

        public FamilleAccueil? ConsulterFamilelAccueil(int familleid)
        {
            FamilleAccueil? fa = _dbContext.Connection.ExecuteReader<FamilleAccueil>(
                "SELECT * FROM FAMILLE_ACCUEIL WHERE id=@p_familleId",
                (r) => r.ToFamilleAccueil(),
                false,
                new { p_familleId = familleid }
                ).FirstOrDefault();

            if (fa is not null)
            {
                fa.Contact = _contactRepository.Consulter(fa.ContactId);
                fa.Animal = Consulter(fa.AniId);
            }
            return fa;
        }

        public FamilleAccueil? FamilleAccueilActuelle(string animalId, bool includeContact)
        {
            FamilleAccueil? fa = _dbContext.Connection.ExecuteReader<FamilleAccueil>(
                "SELECT * FROM fn_lister_familles_accueil_animal(@p_animal_id) WHERE date_fin IS NULL OR date_fin > CURRENT_TIMESTAMP",
                (r) => r.ToFamilleAccueil(),
                false,
                new { p_animal_id = animalId }
            ).FirstOrDefault();

            if (fa != null && includeContact)
            {
                fa.Contact = _contactRepository.Consulter(fa.ContactId);
            }
            return fa;
        }

        public void MettreEnFamilleAccueil(FamilleAccueil familleAccueil)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_mettre_animal_en_famille_accueil", true, new
            {
                p_ani_id = familleAccueil.AniId,
                p_famille_accueil_id = familleAccueil.ContactId,
                p_date_debut = familleAccueil.DateDebut,
                p_date_fin = familleAccueil.DateFin
            });
        }

        public void ModifierFamilleAccueil(FamilleAccueil familleAccueil)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_modifier_famille_accueil", true, new
            {
                p_accueil_id = familleAccueil.Id,
                p_date_debut = familleAccueil.DateDebut,
                p_date_fin = familleAccueil.DateFin
            });
        }

        public IEnumerable<Adoption> ListerAdoptions(string animalId, bool includeContact = false, int offset = 0, int limit = 20)
        {
            List<Adoption> adoptions = _dbContext.Connection.ExecuteReader<Adoption>(
                "SELECT * FROM adoption WHERE ani_id = @p_animal_id ORDER BY date_demande DESC LIMIT @p_limit OFFSET @p_offset",
                (r) => r.ToAdoption(),
                false,
                new
                {
                    p_animal_id = animalId,
                    p_limit = limit,
                    p_offset = offset
                }
            ).ToList();
            if (includeContact)
            {
                List<int> contactIds = adoptions.Select(a => a.ContactId).Distinct().ToList();
                List<Contact> contacts = _contactRepository.ListerParIds(contactIds).ToList();
                foreach (var adoption in adoptions)
                {
                    adoption.Contact = contacts.FirstOrDefault(c => c.Id == adoption.ContactId);
                }
            }
            return adoptions;
        }

        public IEnumerable<Animal> ListerParIds(IEnumerable<string> ids)
        {
            IEnumerable<Animal> animals = Enumerable.Empty<Animal>();
            if (ids.Any())
            {
                animals = _dbContext.Connection.ExecuteReader<Animal>(
                    $"SELECT * FROM vue_animaux WHERE id IN ({String.Join(',', ids.Select(i => $"'{i}'"))})",
                    r => r.ToAnimal(),
                    false
                ).ToList();
            }

            return animals;
        }

        public IEnumerable<Vaccination> ListerVaccination(string animalId)
        {
            return _dbContext.Connection.ExecuteReader<Vaccination>(
                "SELECT va.id as va_id, date, ani_id, vac_id, nom FROM vaccination va " +
                "JOIN vaccin v ON va.vac_id = v.id " +
                "WHERE ani_id = @p_ani_id",
                (r) => r.ToVaccination(),
                false,
                new { p_ani_id = animalId }
            );
        }

        public void VaccinerAnimal(Vaccination vaccination)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_ajouter_vaccination_animal", true, new
            {
                p_ani_id = vaccination.AniId,
                p_vac_id = vaccination.VacId,
                p_date = vaccination.Date
            });
        }

        public void SupprimerVaccination(int vaccinationId)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_supprimer_vaccination_animal", true, new
            {
                p_vaccination_id = vaccinationId
            });
        }
    }
}
