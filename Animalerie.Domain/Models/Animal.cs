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

        public string? Status { get; set; } // Non mappé en base de données

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
            Couleurs = couleurs;
            DateDeces = dateDeces;
            DeletedAt = deletedAt;
        }

        public string ToString()
        {
            return $"\t ID: {Id}\n" +
                   $"\t Nom: {Nom}\n" +
                   $"\t Type: {Type}\n" +
                   $"\t Sexe: {Sexe}\n" +
                   $"\t Particularités: {Particularites}\n" +
                   $"\t Description: {Description}\n" +
                   $"\t Date de stérilisation: {(DateSterilisation.HasValue ? DateSterilisation.Value.ToShortDateString() : "N/A")}\n" +
                   $"\t Date de naissance: {DateNaissance.ToShortDateString()}\n" +
                   $"\t Couleurs: {string.Join(", ", Couleurs)}\n" +
                   $"\t Date de décès: {(DateDeces.HasValue ? DateDeces.Value.ToShortDateString() : "N/A")}\n" +
                   $"\t Status: {Status ?? "N/A"}";
        }

        public string ToStringTableau()
        {
            return $"| {Id,-11} | {Nom,-12} | {Type,-10} | {Sexe,-5} | {DateNaissance.ToShortDateString(),-13} | {Status ?? "N/A"}";
        }
    }
}
