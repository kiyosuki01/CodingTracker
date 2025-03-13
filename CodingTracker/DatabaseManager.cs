
using Microsoft.Data.Sqlite;

namespace CodingTracker
{
    internal class DatabaseManager
    {
        internal void CreateTable(string? connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();
                var tableCmd = connection.CreateCommand();
                tableCmd.CommandText =
                    @"CREATE TABLE IF NOT EXISTS coding (
                        Id INTEGER PRIMARY KEY AUTOINCREMENT,
                        Date TEXT NOT NULL,
                        Duration
                    );";

                tableCmd.ExecuteNonQuery();                
            }
        }
    }
}