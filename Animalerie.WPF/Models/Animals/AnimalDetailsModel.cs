using Animalerie.Domain.CustomEnums.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.WPF.Models.Animals
{
    internal class  AnimalDetailsModel
    {
        public string Id { get; set; }
        public string Nom { get; set; }
        public TypeAnimal Type { get; set; }
        public SexeAnimal Sexe { get; set; }
        public string Particularites { get; set; }
        public string Description { get; set; }
        public DateTime? DateSterilisation { get; set; }
        public DateTime DateNaissance { get; set; }
        public DateTime? DateDeces { get; set; }
        public DateTime? DeletedAt { get; set; }
        public string[] Couleurs { get; set; }

        public string? Status { get; set; }
    }
}
