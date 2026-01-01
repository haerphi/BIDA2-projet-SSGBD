namespace Animalerie.Domain.Models
{
    public class Compatibilite
    {
        public int Id { get; set; }
        public string Type { get; set; }

        public Compatibilite(int id, string type)
        {
            Id = id;
            Type = type;
        }
    }
}
