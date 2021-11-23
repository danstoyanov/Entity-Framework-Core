namespace VaporStore.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Data;
    using Newtonsoft.Json;
    using VaporStore.Data.Models;
    using VaporStore.DataProcessor.Dto.Import;

    public static class Deserializer
    {
        public static string ImportGames(VaporStoreDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonGames = JsonConvert.DeserializeObject<IEnumerable<JsonGameImportModel>>(jsonString);

            foreach (var jsonGame in jsonGames)
            {
                // Check for Invalid Data ! 
                //jsonGame.Tags.Count() == 0
                if (!IsValid(jsonGame) || jsonGame.Tags.Count() == 0)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                // When data is valid !!!
                //
                // Create genre if we don't have or its NULL with that name in Database !
                var currGenre = context.Genres.FirstOrDefault(g => g.Name == jsonGame.Genre)
                    ?? new Genre { Name = jsonGame.Genre };

                // Create devloper of we dont have or its NULL with that name in Database !
                var currDev = context.Developers.FirstOrDefault(d => d.Name == jsonGame.Developer)
                    ?? new Developer { Name = jsonGame.Developer };

                var currGame = new Game()
                {
                    Name = jsonGame.Name,
                    Price = jsonGame.Price,
                    ReleaseDate = jsonGame.ReleaseDate.Value,
                    Developer = currDev,
                    Genre = currGenre,
                };

                // we must check and add the Tags in current object !!! 
                foreach (var jsonTag in jsonGame.Tags)
                {
                    var tag = context.Tags.FirstOrDefault(t => t.Name == jsonTag)
                        ?? new Tag { Name = jsonTag };

                    currGame.GameTags.Add(new GameTag { Tag = tag });
                }

                context.Games.Add(currGame);
                context.SaveChanges();
                result.AppendLine($"Added {jsonGame.Name} ({jsonGame.Genre}) with {jsonGame.Tags.Count()} tags");
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportUsers(VaporStoreDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonUsers = JsonConvert.DeserializeObject<IEnumerable<JsonUserImportModel>>(jsonString);

            foreach (var jsonUser in jsonUsers)
            {
                if (!IsValid(jsonUser) || !jsonUser.Cards.All(IsValid))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var user = new User()
                {
                    FullName = jsonUser.FullName,
                    Username = jsonUser.Username,
                    Email = jsonUser.Email,
                    Age = jsonUser.Age,
                    Cards = jsonUser.Cards.Select(c => new Card
                    {
                        Cvc = c.CVC,
                        Number = c.Number,
                        Type = c.Type.Value,
                    }).ToList()
                };

                context.Users.Add(user);
                context.SaveChanges();
                result.AppendLine($"Imported {user.Username} with {user.Cards.Count()} cards");
            }

            return result.ToString().TrimEnd();
        }

        public static string ImportPurchases(VaporStoreDbContext context, string xmlString)
        {
            var result = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(XmlPurchaseImport[]), new XmlRootAttribute("Purchases"));

            var xmlPurchases = (XmlPurchaseImport[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var xmlPurchase in xmlPurchases)
            {
                // valid check
                if (!IsValid(xmlPurchase))
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                bool parsedDate = DateTime
                    .TryParseExact(xmlPurchase.Date, "dd/MM/yyyy HH:mm", 
                    CultureInfo.InvariantCulture, 
                    DateTimeStyles.None, 
                    out var date);

                if (!parsedDate)
                {
                    result.AppendLine("Invalid Data");
                    continue;
                }

                var purchase = new Purchase()
                {
                    Type = xmlPurchase.Type.Value,
                    ProductKey = xmlPurchase.Key,
                    Date = date
                };

                // game 
                purchase.Game = context.Games.FirstOrDefault(g => g.Name == xmlPurchase.GameName);

                // card
                purchase.Card = context.Cards.FirstOrDefault(c => c.Number == xmlPurchase.Card);

                // check the result ! 
                context.Purchases.Add(purchase);
                context.SaveChanges();

                var currUsername = context.Users.Where(u => u.Id == purchase.Card.UserId).FirstOrDefault();
                result.AppendLine($"Imported {xmlPurchase.GameName} for {currUsername.Username}");
            }

            return result.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}