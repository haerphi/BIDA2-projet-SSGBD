using Animalerie.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Animalerie.DAL.Repositories.Interfaces
{
    public interface ICompatibiliteRepository
    {
        IEnumerable<Compatibilite> Lister();
        Compatibilite? Consulter(int id);
        public void Modifier(Compatibilite compatibilite);
        public Compatibilite Ajouter(Compatibilite compatibilite);
    }
}
