namespace Animalerie.Domain.Models
{
    public class Contact
    {
       
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string? Rue { get; set; }
        public string? Cp { get; set; }
        public string? Localite { get; set; }
        public string RegistreNational { get; set; }
        public string? Gsm { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }

        public Contact(int id, string nom, string prenom, string? rue, string? cp, string? localite, string registreNational, string? gsm, string? telephone, string? email)
        {
            Id = id;
            Nom = nom;
            Prenom = prenom;
            Rue = rue;
            Cp = cp;
            Localite = localite;
            RegistreNational = registreNational;
            Gsm = gsm;
            Telephone = telephone;
            Email = email;
        }

    }
}
