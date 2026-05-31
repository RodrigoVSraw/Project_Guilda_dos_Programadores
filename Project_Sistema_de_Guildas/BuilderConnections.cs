using Npgsql;


namespace BuilderConnections.DAO
{
    public static class BuilderConnection
    {
        private static readonly NpgsqlConnectionStringBuilder connBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "postgres",
            Username = "postgres",
            Password = "",
            SslMode = SslMode.Require,
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

