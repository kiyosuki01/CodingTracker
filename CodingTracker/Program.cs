using System;
using System.Configuration;

namespace CodingTracker
{
    class Program
    {
        static string? connectionString = ConfigurationManager.AppSettings.Get("ConnectionString");
        static void Main(string[] args)
        {
            DatabaseManager databaseManager = new DatabaseManager();
            UserInput userInput = new UserInput();
            databaseManager.CreateTable(connectionString);
            userInput.MainMenu(); 
        }
    }
}