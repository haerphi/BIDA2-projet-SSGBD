using Animalerie.Domain.CustomEnums.Database;

namespace Animalerie.Domain.Models
{
    public class Adoption
    {
        public int Id { get; set; }
        public StatutAdoption Statut { get; set; }
        public DateTime DateDemande { get; set; }
        public string? Note { get; set; }
        public string AniId { get; set; }
        public int ContactId { get; set; }

        // Navigation properties
        public Animal? Animal { get; set; }
        public Contact? Contact { get; set; }

        public Adoption(int id, StatutAdoption statut, DateTime dateDemande, string? note, string aniId, int contactId)
        {
            Id = id;
            Statut = statut;
            DateDemande = dateDemande;
            Note = note;
            AniId = aniId;
            ContactId = contactId;
        }

        public Adoption(string aniId, int contactId, string? note)
        {
            Note = note;
            AniId = aniId;
            ContactId = contactId;
        }

        public override string ToString()
        {
            return $"\t ID: {Id}" +
                $"\t Statut: {Statut.ToString()}" +
                $"\t Date de la demande: {DateDemande}" +
                $"\t Contact: {Contact?.Nom} (ID: {ContactId})" +
                $"\t Note: {Note}";
        }

        public static string TableauEntete()
        {
            return $"{"ID",-5} | {"Contact",-12} | {"Date demande",-22} | {"Statut",-10} | Note";
        }
        public string ToStringTableau()
        {
            return $"{Id,-5} | {Contact?.Nom ?? ContactId.ToString(),-12} | {DateDemande,-22} | {Statut,-10} | {Note}";
        }
    }
}
