using Animalerie.Domain.Models;
using Animalerie.WPF.Models.Contacts;

namespace Animalerie.WPF.Mappers
{
    internal static class ContactMappers
    {
        public static ContactModel ToContactModel(this Contact c)
        {
            return new ContactModel
            {
                Id = c.Id,
                Nom = c.Nom,
                Prenom = c.Prenom,
                Rue = c.Rue,
                Cp = c.Cp,
                Localite = c.Localite,
                RegistreNational = c.RegistreNational,
                Gsm = c.Gsm,
                Telephone = c.Telephone,
                Email = c.Email,
                Roles = c.Roles
            };
        }
    }
}
