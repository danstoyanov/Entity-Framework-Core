namespace SoftJail.DataProcessor
{

    using Data;
    using Newtonsoft.Json;
    using SoftJail.Data.Models;
    using SoftJail.DataProcessor.ImportDto;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.Linq;
    using System.Text;

    public class Deserializer
    {
        public const string ErrorMessageInvalid = "Invalid Data";
        public const string SuccessfullyAddedDepartment = "Imported {0} with {1} cells";
        public const string SuccessfullyAddedPrisoner = "Imported {0} {1} years old";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonDepartments = JsonConvert.DeserializeObject<IEnumerable<JsonDepartmentModel>>(jsonString);

            var departments = new List<Department>();

            foreach (var jsonDepartment in jsonDepartments)
            {
                if (!IsValid(jsonDepartment) || jsonDepartment.Cells.Any(c => c.CellNumber == 0))
                {
                    result.AppendLine(ErrorMessageInvalid);
                    continue;
                }

                var department = new Department()
                {
                    Name = jsonDepartment.Name,
                };

                foreach (var jsonCell in jsonDepartment.Cells)
                {
                    if (!IsValid(jsonCell))
                    {
                        result.AppendLine(ErrorMessageInvalid);
                        continue;
                    }

                    var cell = new Cell()
                    {
                        CellNumber = jsonCell.CellNumber,
                        HasWindow = jsonCell.HasWindow,
                    };

                    department.Cells.Add(cell);
                }

                departments.Add(department);
                result.AppendLine(string.Format(SuccessfullyAddedDepartment, department.Name, department.Cells.Count));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonPrisoners = JsonConvert.DeserializeObject<IEnumerable<JsonPrisonerModel>>(jsonString);

            var prisoners = new List<Prisoner>();

            foreach (var jsonPrisoner in jsonPrisoners)
            {
                if (!IsValid(jsonPrisoner))
                {
                    result.AppendLine(ErrorMessageInvalid);
                    continue;
                }

                DateTime incarcerationDate;
                bool isValidIncDate = DateTime.TryParseExact(jsonPrisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out incarcerationDate);

                if (!isValidIncDate)
                {
                    continue;
                }

                DateTime? releaseDate = null;

                if (jsonPrisoner.ReleaseDate != null)
                {
                    DateTime currRealeaseDate ;

                    bool isValidRealeseDate = DateTime.TryParseExact(jsonPrisoner.ReleaseDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out currRealeaseDate);

                    if (!isValidRealeseDate)
                    {
                        continue;
                    }
                }

                var prisoner = new Prisoner()
                {
                    FullName = jsonPrisoner.FullName,
                    Nickname = jsonPrisoner.Nickname,
                    Age = jsonPrisoner.Age,
                    IncarcerationDate = incarcerationDate,
                    ReleaseDate = releaseDate,
                    CellId = jsonPrisoner.CellId,
                    Bail = jsonPrisoner.Bail
                };

                foreach (var jsonMail in jsonPrisoner.Mails)
                {
                    if (!IsValid(jsonMail))
                    {
                        result.AppendLine(ErrorMessageInvalid);
                        break;
                    }

                    var mail = new Mail()
                    {
                        Description = jsonMail.Description,
                        Sender = jsonMail.Sender,
                        Address = jsonMail.Address
                    };

                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);
                result.AppendLine(string.Format(SuccessfullyAddedPrisoner, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        private static bool IsValid(object obj)
        {
            var validationContext = new System.ComponentModel.DataAnnotations.ValidationContext(obj);
            var validationResult = new List<ValidationResult>();

            bool isValid = Validator.TryValidateObject(obj, validationContext, validationResult, true);
            return isValid;
        }
    }
}