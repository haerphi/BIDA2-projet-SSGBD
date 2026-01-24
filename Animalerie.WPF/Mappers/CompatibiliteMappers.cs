using Animalerie.Domain.Models;
using Animalerie.WPF.Models.Compatibilites;

namespace Animalerie.WPF.Mappers
{
    internal static class CompatibiliteMappers
    {
        public static CompatibiliteModel ToCompatibiliteModel(this Compatibilite a)
        {
            return new CompatibiliteModel
            {
                Id = a.Id,
                Type = a.Type,
            };
        }
    }
}
