using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
using Tools.Database;

namespace Animalerie.DAL.Repositories
{
    public class AdoptionRepository : IAdoptionRepository
    {
        private readonly AnimalerieDBContext _dbContext;
        private readonly IContactRepository _contactRepository;
        private readonly IAnimalRepository _animalRepository;
        public AdoptionRepository(AnimalerieDBContext dbContext, IContactRepository contactRepository, IAnimalRepository animalRepository)
        {
            _dbContext = dbContext;
            _contactRepository = contactRepository;
            _animalRepository = animalRepository;
        }

        public void Ajouter(Adoption adoption)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_ajouter_adoption", true, new
            {
                p_ani_id = adoption.AniId,
                p_adoptant_id = adoption.ContactId,
                p_note = adoption.Note
            });
        }

        public Adoption? Consulter(int adoptionId, bool includeContact = false, bool includeAnimal = false)
        {
            Adoption? adoption = _dbContext.Connection.ExecuteReader<Adoption>(
                "SELECT * FROM adoption WHERE id = @adoptionId",
                (r) => r.ToAdoption(),
                false,
                new { adoptionId }
            ).FirstOrDefault();

            if (adoption is not null)
            {
                if (includeContact)
                {
                    Contact? contact = _contactRepository.Consulter(adoption.ContactId);
                    adoption.Contact = contact;
                }

                if (includeAnimal)
                {
                    Animal? animal = _animalRepository.Consulter(adoption.AniId);
                    adoption.Animal = animal;
                }
            }

            return adoption;
        }

        public void Modifier(Adoption adoption)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_modifier_adoption", true, new
            {
                p_adoption_id = adoption.Id,
                p_nouveau_statut = adoption.Statut,
                p_note = adoption.Note
            });
        }
    }
}
