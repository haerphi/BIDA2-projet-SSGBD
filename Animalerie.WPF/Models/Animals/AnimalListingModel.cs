using Animalerie.Domain.CustomEnums.Database;
using Animalerie.Domain.CustomEnums.ListingFilters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.WPF.Models.Animals
{
    public class AnimalListingModel
    {
        public required string Id { get; set; }
        public required string Nom { get; set; }
        public required TypeAnimal Type {  get; set; }
        public required SexeAnimal Sexe { get; set; }
        public required DateTime DateNaissance { get; set; }
        public string? Status { get; set; }
    }
}
