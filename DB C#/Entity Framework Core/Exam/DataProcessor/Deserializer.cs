namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Plays");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPlaysDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(xmlString);

            ImportPlaysDto[] dtos = (ImportPlaysDto[])xmlSerializer.Deserialize(stringReader);

            List<Play> plays = new List<Play>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }
                

                bool isValidDuration = TimeSpan.TryParseExact(dto.Duration, "c", CultureInfo.InvariantCulture,
                    TimeSpanStyles.None, out TimeSpan validDuration);

                if (!isValidDuration || validDuration.Hours < 1)
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                  if (!Enum.TryParse(typeof(Genre), dto.Genre, out object dtoGenre))
                  {
                      sb.AppendLine(ErrorMessage);
                      continue;
                  }

                

                Play play = new Play()
                {
                    Title = dto.Title,
                    Duration = validDuration,
                    Rating = dto.Rating,
                    Genre = (Genre)dtoGenre,
                    Description = dto.Description,
                    Screenwriter = dto.Screenwriter
                };

                plays.Add(play);
                sb.AppendLine($"Successfully imported {play.Title} with genre {play.Genre} and a rating of {play.Rating}!");
            }
            context.Plays.AddRange(plays);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            StringBuilder sb = new StringBuilder();

            XmlRootAttribute xmlRoot = new XmlRootAttribute("Casts");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCastDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(xmlString);

            ImportCastDto[] dtos = (ImportCastDto[])xmlSerializer.Deserialize(stringReader);

            List<Cast> casts = new List<Cast>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Cast cast = new Cast()
                {
                    FullName = dto.FullName,
                    IsMainCharacter = dto.IsMainCharacter,
                    PhoneNumber = dto.PhoneNumber,
                    PlayId = dto.PlayId
                };
                string character;
                
                if (cast.IsMainCharacter == false)
                {
                    character = "lesser";
                }
                else
                {
                    character = "main";
                }
                casts.Add(cast);
                sb.AppendLine($"Successfully imported actor {cast.FullName} as a {character} character!");
            }
            context.Casts.AddRange(casts);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            StringBuilder sb = new StringBuilder();

            var theatreDtos = JsonConvert.DeserializeObject<IEnumerable<ImportProjectionsDto>>(jsonString);

            foreach (var theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    sb.AppendLine(ErrorMessage);
                    continue;
                }

                Data.Models.Theatre theatre = new Data.Models.Theatre
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director,
                    Tickets = new List<Ticket>()
                };

                context.Theatres.Add(theatre);
                context.SaveChanges();

                foreach (var ticketDto in theatreDto.Tickets)
                {
                    if (!IsValid(ticketDto))
                    {
                        sb.AppendLine(ErrorMessage);
                        continue;
                    }

                    Ticket ticket = new Ticket
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                        TheatreId = theatre.Id
                    };

                    context.Tickets.Add(ticket);
                }

                context.SaveChanges();

                sb.AppendLine(string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count()));
            }

            return sb.ToString().TrimEnd();
        }


        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
