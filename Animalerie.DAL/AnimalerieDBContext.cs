using System.Data.Common;
using Npgsql;

public class AnimalerieDBContext : IDisposable
{
    public DbConnection Connection { get; private set; }

    public AnimalerieDBContext(string connString)
    {
        Connection = new NpgsqlConnection(connString);
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
    }
}