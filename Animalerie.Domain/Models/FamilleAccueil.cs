namespace Animalerie.Domain.Models
{
    public class FamilleAccueil
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string AniId { get; set; }
        public int FamilleAccueilId { get; set; }
    }
}
