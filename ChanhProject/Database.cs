using Microsoft.Data.Sqlite;

namespace ChanhProject
{
    public class Database
    {
        private static SqliteConnection connection = null;

        public static SqliteConnection GetConnection()
        {
            if (connection == null)
            {
                connection = new SqliteConnection("" +
                    new SqliteConnectionStringBuilder
                    {
                        DataSource = "library.db"
                    });
                connection.Open();
            }
            return connection;
        }
    }
}
