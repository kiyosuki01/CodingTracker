using System.Configuration;
using Microsoft.Data.Sqlite;
using Spectre.Console;
using Dapper;

namespace CodingTracker
{
    internal class CodingController
    {
        string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");

        internal List<CodingSessions> Get()
        {
            List<CodingSessions> tableData = new List<CodingSessions>();
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = "SELECT * FROM coding";

                    using (var reader = tableCmd.ExecuteReader())
                    {
                        if (reader.HasRows)
                        {
                            while (reader.Read())
                            {
                                tableData.Add(
                                new CodingSessions
                                {
                                    Id = reader.GetInt32(0),
                                    Date = reader.GetString(1),
                                    Duration = reader.GetString(2)
                                });
                            }
                        }
                    }
                }
            }

            TableVisualisation.ShowTable(tableData);

            return tableData;
        }

        internal CodingSessions? GetById(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                string query = "SELECT * FROM coding WHERE Id = @Id";
                return connection.QueryFirstOrDefault<CodingSessions>(query, new { Id = id });
            }
        }


        internal void Post(CodingSessions codingSessions)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"INSERT INTO coding (date, duration) VALUES ('{codingSessions.Date}', '{codingSessions.Duration}');";
                    tableCmd.ExecuteNonQuery();
                }
            }
        }

        internal void Delete(int id)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"DELETE from coding WHERE Id = '{id}'";
                    tableCmd.ExecuteNonQuery();

                    AnsiConsole.Markup($"[green]Record with Id {id} was deleted[/]");
                }
            }
        }

        internal void Update(CodingSessions codingSessions)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                using (var tableCmd = connection.CreateCommand())
                {
                    connection.Open();
                    tableCmd.CommandText = $"UPDATE coding SET Date = '{codingSessions.Date}', Duration = '{codingSessions.Duration}' WHERE Id = '{codingSessions.Id}'";
                    tableCmd.ExecuteNonQuery();

                    AnsiConsole.Markup($"[green]Record with Id {codingSessions.Id} was updated[/]");
                }
            }
        }
    }
}