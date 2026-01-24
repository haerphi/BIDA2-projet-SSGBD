using Animalerie.WPF.Models.Contacts;

namespace Animalerie.WPF.Models.Animals
{
    internal class FamilleAccueilModel
    {
        public int Id { get; set; }
        public DateTime DateDebut { get; set; }
        public DateTime? DateFin { get; set; }
        public string AniId { get; set; }
        public int ContactId { get; set; }

        public AnimalListingModel Animal { get; set; }
        public ContactModel Contact { get; set; }
    }
}
