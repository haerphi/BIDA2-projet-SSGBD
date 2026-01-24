using Animalerie.Domain.Models;
using Animalerie.WPF.Models.Animals;
using Animalerie.WPF.Models.Compatibilites;

namespace Animalerie.WPF.Mappers
{
    internal static class AnimalMappers
    {
        public static AnimalListingModel ToAnimalListingModel(this Animal a)
        {
            return new AnimalListingModel
            {
                Id = a.Id,
                Nom = a.Nom,
                Type = a.Type,
                Sexe = a.Sexe,
                DateNaissance = a.DateNaissance,
                Status = a.Status,
            };
        }

        public static AnimalDetailsModel ToAnimalDetailsModel(this Animal a)
        {
            return new AnimalDetailsModel
            {
                Id = a.Id,
                Nom = a.Nom,
                Type = a.Type,
                Sexe = a.Sexe,
                Particularites = a.Particularites,
                Description = a.Description,
                DateSterilisation = a.DateSterilisation,
                DateNaissance = a.DateNaissance,
                DateDeces = a.DateDeces,
                DeletedAt = a.DeletedAt,
                Couleurs = a.Couleurs,
                Status = a.Status,
            };
        }

        public static CompatibiliteModel ToCompatibiliteModel(this Compatibilite a)
        {
            return new CompatibiliteModel
            {
                Id = a.Id,
                Type = a.Type,
            };
        }

        public static AnimalCompatibiliteModel ToAnimalCompatibiliteModel(this AniCompatibilite ac)
        {
            return new AnimalCompatibiliteModel
            {
                Compatibilite = ac.Compatibilite.ToCompatibiliteModel(),
                AniId = ac.AniId,
                Valeur = ac.Valeur,
                Description = ac.Description,
                UpdatedAt = ac.UpdatedAt,
            };
        }
    }
}
