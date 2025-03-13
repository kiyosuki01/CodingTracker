using ConsoleTableExt;
using Spectre.Console;

namespace CodingTracker
{
    internal class TableVisualisation
    {
        internal static void ShowTable<T>(List<T> tableData) where T : class
        {
            if (tableData == null || !tableData.Any())
            {
                AnsiConsole.MarkupLine("[bold red]No rows found![/]");
                Console.ReadKey();
                return;
            }

            var table = new Table();

            var properties = typeof(T).GetProperties();
            foreach (var prop in properties)
            {
                table.AddColumn(new TableColumn(prop.Name).Centered());
            }

            foreach (var item in tableData)
            {
                var values = properties.Select(prop => prop.GetValue(item)?.ToString() ?? "N/A");
                table.AddRow(values.ToArray());
            }

            table.Title("[bold yellow]Coding Sessions[/]");
            table.Border(TableBorder.Rounded);

            AnsiConsole.Write(table);
            Console.ReadKey();
        }
    }
}