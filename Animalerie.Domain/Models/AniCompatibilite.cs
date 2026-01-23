namespace Animalerie.Domain.Models
{
    public class AniCompatibilite
    {
        public Compatibilite Compatibilite { get; set; } 
        public string AniId { get; set; }
        public bool Valeur { get; set; }
        public string? Description { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AniCompatibilite(Compatibilite compatibilite, string aniId, bool valeur, string? description, DateTime updatedAt)
        {
            Valeur = valeur;
            Description = description;
            Compatibilite = compatibilite;
            AniId = aniId;
            UpdatedAt = updatedAt;
        }
    }
}
