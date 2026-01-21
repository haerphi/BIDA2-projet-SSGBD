using Animalerie.Domain.Models;
using Animalerie.WPF.Models.Animals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
