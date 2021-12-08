namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ExportDto;

    

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .Where(t => t.NumberOfHalls >= numbersOfHalls
                       && t.Tickets.Count >= 20)
                .Select(t => new
                {
                    Name = t.Name,
                    Halls = t.NumberOfHalls,
                    TotalIncome = t.Tickets
                        .Where(tk => tk.RowNumber >= 1 && tk.RowNumber <= 5)
                        .Sum(tk => tk.Price),
                    Tickets = t.Tickets
                        .Where(tc => tc.RowNumber >= 1 && tc.RowNumber <= 5)
                        .OrderByDescending(tc => tc.Price)
                        .Select(tc => new
                        {
                            Price = tc.Price,
                            RowNumber = tc.RowNumber
                        })
                        .ToArray()
                })
                .OrderByDescending(t => t.Halls)
                .ThenBy(t => t.Name)
                .ToArray();

            string json = JsonConvert.SerializeObject(theatres, Formatting.Indented);

            return json;
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var sb = new StringBuilder();
            using StringWriter sw = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Plays");

            XmlSerializer xmlSerialier = new XmlSerializer(typeof(Plays[]), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            var plays = context.Plays
                .Where(x => x.Rating <= rating)
                .OrderBy(o => o.Title)
                .ThenByDescending(o => o.Genre)
                .Select(x => new Plays
                {
                    Title = x.Title,
                    Duration = x.Duration.ToString("c"),
                    Rating = x.Rating == 0 ? "Premier" : x.Rating.ToString(),
                    Genre = x.Genre.ToString(),
                    Actors = x.Casts
                        .Where(a => a.IsMainCharacter)
                        .Select(a => new Actors
                        {
                            FullName = a.FullName,
                            MainCharacter = $"Plays main character in '{a.Play.Title}'."
                        })
                        .OrderByDescending(o => o.FullName)
                        .ToArray()
                })
                .ToArray();

            xmlSerialier.Serialize(sw, plays, namespaces);

            return sb.ToString().TrimEnd();
        }
    }
}
