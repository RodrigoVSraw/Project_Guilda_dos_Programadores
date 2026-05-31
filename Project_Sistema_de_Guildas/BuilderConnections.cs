using Npgsql;


namespace BuilderConnections.DAO
{
    public static class BuilderConnection
    {
        private static readonly NpgsqlConnectionStringBuilder connBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "db.lnvcvnpjifmqmtfifoqy.supabase.co",
            Port = 5432,
            Database = "postgres",
            Username = "postgres",
            Password = "guildaDosProgramadores",
            SslMode = SslMode.Disable,
            Pooling = false,
            MaxPoolSize = 100,
            ConnectionIdleLifetime = 100
        };

        public static string GetConnectionString()
        {
            return connBuilder.ToString();
        }

    }
}

