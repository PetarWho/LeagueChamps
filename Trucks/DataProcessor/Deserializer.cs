namespace Trucks.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;
    using Data;
    using LeagueChampsDb.Data.Models;
    using LeagueChampsDb.Data.Models.Enums;
    using LeagueChampsDb.DataProcessor.ImportDto;
    using Newtonsoft.Json;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data for {0}";

        private const string SuccessfullyImportedChampion = "Successfully imported champion - {0}.";


        public static string ImportChampions(LeagueChampsContext context, string jsonString)
        {
            var dtos = JsonConvert.DeserializeObject<ImportChampionDto[]>(jsonString);
            var sb = new StringBuilder();

            var champions = new List<Champion>();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name));
                    continue;
                }

                bool isDefinedPositionEnum = Enum.TryParse(dto.Position, out Position position);
                bool isDefinedClassEnum = Enum.TryParse(dto.Class, out PlaystyleClass playstyleClass);

                if(!isDefinedClassEnum || !isDefinedPositionEnum)
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name + " - enums"));
                    continue;
                }

                bool isValidDate = DateTime.TryParseExact(dto.ReleaseDate, "yyyy-MM-dd", 
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime date);

                if (!isValidDate)
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name + " - date"));
                    continue;
                }

                if (dto.PriceBE != 450 && dto.PriceBE != 1350 && dto.PriceBE != 3150 && dto.PriceBE != 4800 && dto.PriceBE != 6300)
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name + " - PriceBE"));
                    continue;
                }

                if (dto.PriceRP != 260 && dto.PriceRP != 585 && dto.PriceRP != 790 && dto.PriceRP != 880 && dto.PriceRP != 975)
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name + " - PriceRP"));
                    continue;
                }

                champions.Add(new Champion()
                {
                    Name = dto.Name,
                    Description = dto.Description,
                    ImageUrl = dto.Image,
                    PriceBE = dto.PriceBE,
                    PriceRP = dto.PriceRP,
                    HasMana = dto.HasMana,
                    IsRanged = dto.IsRanged,
                    Position = position,
                    Class = playstyleClass,
                    ReleaseDate = date
                });
                sb.AppendLine(String.Format(SuccessfullyImportedChampion, dto.Name));
            }
            context.AddRange(champions);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportUser(LeagueChampsContext context, string jsonString)
        {
            var dtos = JsonConvert.DeserializeObject<ImportUserDto[]>(jsonString);
            var sb = new StringBuilder();

            var users = new List<User>();

            var champions = context.Champions.ToHashSet();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Username));
                }

                var registredUsers = context.Users.ToList();

                if(registredUsers.Any(u=> u.Username == dto.Username))
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Username + " already exists"));
                }

                if (registredUsers.Any(u => u.Email == dto.Email))
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Email + " already exists"));
                }

                var user = new User()
                {
                    Username = dto.Username,
                    Email = dto.Email,
                    Password = dto.Password
                };


                foreach (var champ in champions)
                {
                    user.UsersChampions.Add(new UserChampion()
                    {
                        Champion = champ,
                        User = user
                    });
                }

                users.Add(user);

                sb.AppendLine($"Successfully inserted user {dto.Username} with {user.UsersChampions.Count} champions");
            }

            context.AddRange(users);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        public static string ImportAbility(LeagueChampsContext context, string jsonString)
        {
            var dtos = JsonConvert.DeserializeObject<ImportAbilityDto[]>(jsonString);
            var sb = new StringBuilder();

            var abilities = new List<Ability>();

            var champions = context.Champions.ToHashSet();

            foreach (var dto in dtos)
            {
                if (!IsValid(dto))
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name));
                }

                if (context.Abilities.Any(a => a.Name == dto.Name))
                {
                    sb.AppendLine(String.Format(ErrorMessage, dto.Name + " already exists"));
                }

                var ability = new Ability()
                {
                    Name = dto.Name,
                    ImageURL = dto.ImageURL,
                    IsPassive = dto.IsPassive
                };


                foreach (var champ in champions)
                {
                    if(champ.Name.ToLower().Trim() == dto.Champion.ToLower().Trim())
                    {
                        champ.Abilities.Add(ability);
                    }
                }

                abilities.Add(ability);

            }
            sb.AppendLine($"Successfully inserted {abilities.Count} abilities");

            context.AddRange(abilities);
            context.SaveChanges();

            return sb.ToString().TrimEnd();
        }

        private static bool IsValid(object dto)
        {
            var validationContext = new ValidationContext(dto);
            var validationResult = new List<ValidationResult>();

            return Validator.TryValidateObject(dto, validationContext, validationResult, true);
        }
    }
}
