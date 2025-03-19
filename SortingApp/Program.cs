using System;
using System.Collections.Generic;
using System.IO;
using Spectre.Console;

namespace SortingApp
{
    public class Participant
    {
        public string Name { get; set; }
        public decimal Time { get; set; }
        public int Place { get; set; }
    }

    public class Program
    {
        public static void Main()
        {
            SendTemplate();
            Console.Write("Путь к файлу: ");
            var filePath = Console.ReadLine();

            var participants = ReadParticipants(filePath!);
            var sortedParticipants = SortParticipants(participants);

            PrintResults(sortedParticipants);
        }

        public static void SendTemplate()
        {
            AnsiConsole.MarkupLine("[white]Введите полный путь к файлу, который нужно отсортировать.[/]");
            AnsiConsole.MarkupLine("[white]Путь можно найти, если нажать по файлу ПКМ --> открыть \"Свойства\" --> скопировать значение \"Расположение\". В конец нужно добавить \\имя_файла.txt[/]");
            AnsiConsole.MarkupLine("\n[lightgreen]Пример правильно введенного расположения:\nC:\\Users\\nikit\\OneDrive\\Рабочий стол\\Данные.txt[/]\n");
            AnsiConsole.MarkupLine("[white]В файле для сортировки данные должны быть записаны по следующему шаблону:[/]");
            AnsiConsole.MarkupLine("[lightgreen]Иванов Иван Иванович:10,2\nХарламов Никита Владимирович:15,3[/]\n");
            AnsiConsole.MarkupLine("[white]Сначала идет ФИО, а затем через двоеточие результат проплыва в секундах.[/]\n");
        }

        public static List<Participant> ReadParticipants(string filePath)
        {
            try
            {
                var lines = File.ReadAllLines(filePath);
                var participants = new List<Participant>();

                foreach (var line in lines)
                {
                    var parts = line.Split(':');
                    participants.Add(new Participant
                    {
                        Name = parts[0].Trim(),
                        Time = decimal.Parse(parts[1].Trim())
                    });
                }

                return participants;
            }
            catch (Exception ex)
            {
                AnsiConsole.MarkupLine($"[red]Ошибка чтения файла: {ex.Message}[/]");
                return null;
            }
        }

        public static List<Participant> SortParticipants(List<Participant> participants)
        {
            if (participants == null || !participants.Any())
            {
                return new List<Participant>();
            }

            // Сортируем участников по времени
            var sortedParticipants = participants.OrderBy(x => x.Time).ToList();

            // Присваиваем места участникам
            int currentPlace = 1;
            for (int i = 0; i < sortedParticipants.Count; i++)
            {
                if (sortedParticipants[i].Place == 0 && sortedParticipants.Any(x => x.Time == sortedParticipants[i].Time && x.Name != sortedParticipants[i].Name))
                {
                    foreach (var sameParticipant in sortedParticipants.Where(x => x.Time == sortedParticipants[i]?.Time))
                    {
                        sameParticipant.Place = currentPlace;
                    }
                    currentPlace++;
                }
                else if (sortedParticipants[i].Place == 0)
                {
                    sortedParticipants[i].Place = currentPlace;
                    currentPlace++;
                }
            }

            return sortedParticipants;
        }

        public static void PrintResults(List<Participant> participants)
        {
            if (participants == null || !participants.Any())
            {
                AnsiConsole.MarkupLine("[red]Нет данных для отображения.[/]");
                return;
            }

            var table = new Table();
            table.AddColumn(new TableColumn("ФИО").LeftAligned());
            table.AddColumn(new TableColumn("Время").Centered());
            table.AddColumn(new TableColumn("Место").Centered());

            AnsiConsole.MarkupLine("\n[lightgreen]Результаты:[/]\n");
            foreach (var participant in participants)
            {
                table.AddRow(
                    new Markup(participant.Name),
                    new Markup(participant.Time.ToString()),
                    new Markup(participant.Place.ToString())
                );
            }

            AnsiConsole.Write(table);
        }
    }
}