using Npgsql;


namespace BuilderConnections.DAO
{
    public static class BuilderConnection
    {
        private static readonly NpgsqlConnectionStringBuilder connBuilder = new NpgsqlConnectionStringBuilder
        {
            Host = "aws-1-sa-east-1.pooler.supabase.com",
            Port = 6543,
            Database = "postgres",
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

