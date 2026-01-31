using Animalerie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.WPF.Models.Contacts
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string Nom { get; set; }
        public string Prenom { get; set; }
        public string NomComplet => $"{Prenom} {Nom}";
        public string? Rue { get; set; }
        public string? Cp { get; set; }
        public string? Localite { get; set; }
        public string RegistreNational { get; set; }
        public string? Gsm { get; set; }
        public string? Telephone { get; set; }
        public string? Email { get; set; }

        public List<PersonneRole> Roles { get; set; } = new();
    }
}
