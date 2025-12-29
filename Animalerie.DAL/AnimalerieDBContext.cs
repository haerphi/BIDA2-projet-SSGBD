using Animalerie.Domain.CustomEnums.Database;
using Npgsql;
using System.Data.Common;

public class AnimalerieDBContext : IDisposable
{
    private readonly NpgsqlDataSource _dataSource;
    public DbConnection Connection { get; private set; }

    public AnimalerieDBContext(string connString)
    {
        // Création d'un builder pour mapper les types PostgreSQL vers les Enums C#
        var dataSourceBuilder = new NpgsqlDataSourceBuilder(connString);

        dataSourceBuilder.MapEnum<RaisonEntree>("raison_entree");
        dataSourceBuilder.MapEnum<RaisonSortie>("raison_sortie");
        dataSourceBuilder.MapEnum<RoleNom>("role_nom");
        dataSourceBuilder.MapEnum<SexeAnimal>("sexe_animal");
        dataSourceBuilder.MapEnum<StatutAdoption>("statut_adoption");
        dataSourceBuilder.MapEnum<TypeAnimal>("type_animal");

        _dataSource = dataSourceBuilder.Build();
        Connection = _dataSource.CreateConnection();
    }

    public static AnimalerieDBContext Build(string connString)
    {
        return new AnimalerieDBContext(connString);
    }

    public void Connect()
    {
        if (Connection.State != System.Data.ConnectionState.Open)
        {
            Connection.Open();
        }
    }

    public void Dispose()
    {
        Connection?.Dispose();
        _dataSource?.Dispose();
    }
}