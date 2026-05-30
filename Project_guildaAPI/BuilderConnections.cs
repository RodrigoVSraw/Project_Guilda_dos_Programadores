using Npgsql;


namespace BuilderConnections.DAO
{
    public static class BuilderConnection
    {
        private static readonly NpgsqlConnectionStringBuilder connBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "localhost",
            Port = 5432,
            Database = "guilda_db",
            Username = "",
            Password = "",
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

