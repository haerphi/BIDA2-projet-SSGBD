using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
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

        public void AjouterAnimal(Animal animal, string[] couleurs, Contact contact)
        {
            _dbContext.Connection.ExecuteNonQuery("ps_ajouter_animal", true, new
            {
                p_id = animal.Id,
                p_nom = animal.Nom,
                p_type = animal.Type,
                p_sexe = animal.Sexe,
                p_date_naissance = animal.DateNaissance,
                p_couleurs = couleurs,
                p_contact_id = contact.Id
            });
        }

        public Animal? ConsulterAnimal(string id)
        {
            throw new NotImplementedException();
        }

        public void ModifierCompatibilite(string aniId, int compId, bool valeur, string? desc = null)
        {
            throw new NotImplementedException();
        }

        public void SupprimerAnimal(string id)
        {
            throw new NotImplementedException();
        }
    }
}
