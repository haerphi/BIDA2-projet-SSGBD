namespace Animalerie.Domain.Models
{
    public class AniCompatibilite
    {
        public bool Valeur { get; set; }
        public string Description { get; set; }
        public int CompId { get; set; }
        public string CompType { get; set; }
        public string AniId { get; set; }
        public DateTime UpdatedAt { get; set; }

        public AniCompatibilite(bool valeur, string description, int compId, string compType, string aniId, DateTime updatedAt)
        {
            Valeur = valeur;
            Description = description;
            CompId = compId;
            CompType = compType;
            AniId = aniId;
            UpdatedAt = updatedAt;
        }
    }
}
