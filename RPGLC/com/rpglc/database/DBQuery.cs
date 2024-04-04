using System.Configuration;
using System.Data.Common;
using System.Data.SqlClient;

namespace com.rpglc.database;
public static class DBQuery {
    private static readonly string provider = ConfigurationManager.AppSettings["provider"];
    private static readonly string connectionString = ConfigurationManager.AppSettings["connectionString"];

    private static bool initialized = false;

    private static void Initialize() {
        DbProviderFactories.RegisterFactory(provider, SqlClientFactory.Instance);
        initialized = true;
    }

    private static void ExecuteCommandNoResults(string commandText) {
        if (!initialized) {
            Initialize();
        }

        DbProviderFactory factory = DbProviderFactories.GetFactory(provider);
        
        using DbConnection connection = factory.CreateConnection();

        if (connection == null) {
            Console.WriteLine("Failed to connect to database.");
            Console.ReadLine();
            return;
        }

        connection.ConnectionString = connectionString;
        connection.Open();

        DbCommand command = factory.CreateCommand();
        if (command == null) {
            Console.WriteLine("Failed to create command.");
            Console.ReadLine();
            return;
        }
        command.Connection = connection;
        command.CommandText = commandText;

        try {
            command.ExecuteReader();
        } catch (SqlException) {

        }
    }

    public static void InsertEffectTemplate(string datapackNamespace, string id, string name, string description, string subeventFilters) {
        ExecuteCommandNoResults($"""
                INSERT INTO EffectTemplate (
                    Namespace,
                    Id,
                    Name,
                    Description,
                    SubeventFilters
                )
                VALUES (
                    '{datapackNamespace}',
                    '{id}',
                    '{name}',
                    '{description}',
                    '{subeventFilters}'
                );
                """
        );
    }
}
