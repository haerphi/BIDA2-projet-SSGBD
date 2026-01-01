namespace Animalerie.Domain.Models
{
    public class FamilleAccueil
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string AniId { get; set; }
        public int ContactId { get; set; }

        public Animal? Animal { get; set; }
        public Contact? Contact { get; set; }

        public FamilleAccueil(int id, DateTime dateDebut, DateTime? dateFin, string aniId, int contactId)
        {
            Id = id;
            DateDebut = dateDebut;
            DateFin = dateFin;
            AniId = aniId;
            ContactId = contactId;
        }
    }
}
