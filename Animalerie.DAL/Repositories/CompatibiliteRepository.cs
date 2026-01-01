using Animalerie.DAL.Mappers;
using Animalerie.DAL.Repositories.Interfaces;
using Animalerie.Domain.Models;
using Tools.Database;

namespace Animalerie.DAL.Repositories
{
    public class CompatibiliteRepository : ICompatibiliteRepository
    {
        AnimalerieDBContext _dbContext;

        public CompatibiliteRepository(AnimalerieDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public IEnumerable<Compatibilite> Lister()
        {
            return _dbContext.Connection.ExecuteReader<Compatibilite>("SELECT * FROM compatibilite", (r) => r.ToCompatibilite());
        }

        public Compatibilite? Consulter(int id)
        {
            return _dbContext.Connection.ExecuteReader<Compatibilite>(
                "SELECT * FROM compatibilite WHERE id = @id", 
                (r) => r.ToCompatibilite(),
                false,
                new { id }
             ).FirstOrDefault();
        }
    }
}
