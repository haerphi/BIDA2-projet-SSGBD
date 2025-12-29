using Animalerie.Domain.CustomEnums.Database;

namespace Animalerie.Domain.Models
{
    public class Animal
    {
        public string Id { get; set; } // CHAR(11)
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
        
        public Animal(string id, string nom, TypeAnimal type, SexeAnimal sexe, string particularites, string description, DateTime? dateSterilisation, DateTime dateNaissance, string[] couleurs, DateTime? dateDeces = null, DateTime? deletedAt = null)
        {
            Id = id;
            Nom = nom;
            Type = type;
            Sexe = sexe;
            Particularites = particularites;
            Description = description;
            DateSterilisation = dateSterilisation;
            DateNaissance = dateNaissance;
            DateDeces = dateDeces;
            DeletedAt = deletedAt;
            Couleurs = couleurs;
        }
    }
}
