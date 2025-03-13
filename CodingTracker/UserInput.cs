using System.Globalization;
using Spectre.Console;

namespace CodingTracker
{
    internal class UserInput
    {
        CodingController codingController = new CodingController();

        internal void MainMenu()
        {
            while (true)
            {
                AnsiConsole.Clear();
                var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .Title("[bold white]---[/][bold green] Main Menu [/][bold white]---[/]")
                .AddChoices("View All Records", "Add Record", "Delete Record", "Update Record", "Exit"));

                switch (choice)
                {
                    case "View All Records":
                        codingController.Get();
                        break;
                    case "Add Record":
                        AddRecord();
                        break;
                    case "Delete Record":
                        DeleteRecord();
                        break;
                    case "Update Record":
                        UpdateRecord();
                        break;
                    case "Exit":
                        return;
                }
            }
        }

        private void AddRecord()
        {
            var date = GetDateInput();
            var duration = GetDurationInput();

            CodingSessions codingSessions = new CodingSessions();

            codingSessions.Date = date;
            codingSessions.Duration = duration;

            codingController.Post(codingSessions);
        }

        private void DeleteRecord()
        {
            AnsiConsole.Clear();
            codingController.Get();

            int idInput = AnsiConsole.Ask<int>("[yellow]Enter ID of the session to delete (or 0 to return): [/]");

            if (idInput == 0) return;

            var session = codingController.GetById(idInput);

            if (session == null)
            {
                AnsiConsole.Markup("[red]Error: ID not found![/]\n");
                Console.ReadKey();
                return;
            }

            codingController.Delete(idInput);
            Console.ReadKey();
        }


        private void UpdateRecord()
        {
            AnsiConsole.Clear();
            codingController.Get();

            int idInput = AnsiConsole.Ask<int>("[yellow]Enter ID of the session to update (or 0 to return): [/]");

            if (idInput == 0) return;

            var codingSession = codingController.GetById(idInput);

            if (codingSession == null)
            {
                AnsiConsole.Markup("[red]Error: ID not found![/]\n");
                Console.ReadKey();
                return;
            }

            while (true)
            {
                string choice = AnsiConsole.Prompt(
                    new SelectionPrompt<string>()
                        .Title("What would you like to update?")
                        .AddChoices("Date", "Duration", "Return to Main Menu"));

                switch (choice)
                {
                    case "Date":
                        codingSession.Date = GetDateInput();
                        break;
                    case "Duration":
                        codingSession.Duration = GetDurationInput();
                        break;
                    case "Return to Main Menu":
                        return;
                }

                codingController.Update(codingSession);
                AnsiConsole.Markup("[green]Session updated successfully![/]\n");
            }
        }


        internal string GetDateInput()
        {
            while (true)
            {
                string dateInput = AnsiConsole.Ask<string>("[yellow]Enter date (dd-MM-yy) or 0 to return to Main Menu:[/]");

                if (dateInput == "0")
                {
                    return "";
                }

                if (DateTime.TryParseExact(dateInput, "dd-MM-yy", new CultureInfo("en-US"), DateTimeStyles.None, out _))
                {
                    return dateInput;
                }

                AnsiConsole.MarkupLine("[red]Invalid date! Try again.[/]");
            }
        }

        internal string GetDurationInput()
        {
            while (true)
            {
                string durationInput = AnsiConsole.Ask<string>("[yellow]Enter duration (hh:mm) or 0 to return to Main Menu:[/]");

                if (durationInput == "0")
                {
                    return "";
                }

                if (TimeSpan.TryParseExact(durationInput, "h\\:mm", CultureInfo.InvariantCulture, out TimeSpan parsedDuration))
                {

                    if (parsedDuration < TimeSpan.Zero)
                    {
                        AnsiConsole.MarkupLine("[red]Negative Time is not accepted! Try again.[/]");
                        continue;
                    }

                    return durationInput
                    ;
                }

                AnsiConsole.MarkupLine("[red]Invalid duration! Try again.[/]");
            }
        }
    }
}