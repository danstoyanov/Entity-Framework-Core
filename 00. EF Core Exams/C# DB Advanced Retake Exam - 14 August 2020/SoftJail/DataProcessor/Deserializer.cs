namespace SoftJail.DataProcessor
{
    using Data;
    using System;
    using System.IO;
    using System.Text;
    using System.Linq;
    using System.Globalization;
    using System.Xml.Serialization;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using Newtonsoft.Json;

    using SoftJail.Data.Models;
    using SoftJail.Data.Models.Enums;
    using SoftJail.DataProcessor.ImportDto;

    public class Deserializer
    {
        public const string INVALID_INPUT_DATA = "Invalid Data";
        public const string SUCCESSFULLY_IMPORTED_DEPARTMENTS = "Imported {0} with {1} cells";
        public const string SUCCESSFULLY_IMPORTED_PRISONER = "Imported {0} {1} years old";
        public const string SUCCESSFULLY_IMPORTED_OFFICER = "Imported {0} ({1} prisoners)";

        public static string ImportDepartmentsCells(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonDepartments = JsonConvert.DeserializeObject<IEnumerable<JsonDepartmnetModel>>(jsonString);

            var departments = new HashSet<Department>();

            foreach (var jsonDepartment in jsonDepartments)
            {
                if (!IsValid(jsonDepartment) || jsonDepartment.Cells.Any(c => c.CellNumber == 0 || c.CellNumber > 1000))
                {
                    result.AppendLine(INVALID_INPUT_DATA);
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
                        result.AppendLine(INVALID_INPUT_DATA);
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
                result.AppendLine(string.Format(SUCCESSFULLY_IMPORTED_DEPARTMENTS, department.Name, department.Cells.Count));
            }

            context.Departments.AddRange(departments);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportPrisonersMails(SoftJailDbContext context, string jsonString)
        {
            var result = new StringBuilder();

            var jsonPrisoners = JsonConvert.DeserializeObject<IEnumerable<JsonPrisonerModel>>(jsonString);

            var prisoners = new HashSet<Prisoner>();

            foreach (var jsonPrisoner in jsonPrisoners)
            {
                if (!IsValid(jsonPrisoner))
                {
                    result.AppendLine(INVALID_INPUT_DATA);
                    continue;
                }

                DateTime incarDate;
                bool isValidIncarDate = DateTime.TryParseExact(jsonPrisoner.IncarcerationDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out incarDate);


                DateTime realeseDate;

                bool isValidRealeaseDate = DateTime.TryParseExact(jsonPrisoner.IncarcerationDate, "dd/MM/yyyy",
                    CultureInfo.InvariantCulture, DateTimeStyles.None, out realeseDate);

                var prisoner = new Prisoner()
                {
                    FullName = jsonPrisoner.FullName,
                    Nickname = jsonPrisoner.Nickname,
                    Age = jsonPrisoner.Age,
                    IncarcerationDate = incarDate,
                    ReleaseDate = isValidIncarDate ? (DateTime?)realeseDate : null,
                    Bail = jsonPrisoner.Bail,
                    CellId = jsonPrisoner.CellId,
                };

                foreach (var jsonMail in jsonPrisoner.Mails)
                {
                    if (!IsValid(jsonMail))
                    {
                        result.AppendLine(INVALID_INPUT_DATA);
                        continue;
                    }

                    var mail = new Mail()
                    {
                        Description = jsonMail.Description,
                        Sender = jsonMail.Sender, 
                        Address = jsonMail.Address,
                    };

                    prisoner.Mails.Add(mail);
                }

                prisoners.Add(prisoner);
                result.AppendLine(string.Format(SUCCESSFULLY_IMPORTED_PRISONER, prisoner.FullName, prisoner.Age));
            }

            context.Prisoners.AddRange(prisoners);
            context.SaveChanges();

            return result.ToString().TrimEnd();
        }

        public static string ImportOfficersPrisoners(SoftJailDbContext context, string xmlString)
        {
            var result = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(XmlOfficerModel[]), new XmlRootAttribute("Officers"));

            var xmlOfficers = (XmlOfficerModel[])xmlSerializer.Deserialize(new StringReader(xmlString));

            var officers = new HashSet<Officer>();

            foreach (var xmlOfficer in xmlOfficers)
            {
                if (!IsValid(xmlOfficer))
                {
                    result.AppendLine(INVALID_INPUT_DATA);
                    continue;
                }

                // Parse the enums value !
                Position position;
                var isPositionParsed = Enum.TryParse<Position>(xmlOfficer.Position, out position);

                if (!isPositionParsed)
                {
                    result.AppendLine(INVALID_INPUT_DATA);
                    continue;
                }

                Weapon weapon;
                var isWeaponParsed = Enum.TryParse<Weapon>(xmlOfficer.Weapon, out weapon);

                if (!isWeaponParsed)
                {
                    result.AppendLine(INVALID_INPUT_DATA);
                    continue;
                }

                var officer = new Officer()
                {
                    FullName = xmlOfficer.Name,
                    Salary = xmlOfficer.Salary,
                    Position = position,
                    Weapon = weapon,
                    DepartmentId = xmlOfficer.DepartmentId,
                };

                foreach (var xmlPrisonerId in xmlOfficer.Prisoners)
                {
                    var officerPrisoner = new OfficerPrisoner()
                    {
                        Officer = officer,
                        PrisonerId = xmlPrisonerId.PrisonerId
                    };

                    officer.OfficerPrisoners.Add(officerPrisoner);
                }

                officers.Add(officer);
                result.AppendLine(string.Format(SUCCESSFULLY_IMPORTED_OFFICER, officer.FullName, officer.OfficerPrisoners.Count));
            }

            context.AddRange(officers);
            context.SaveChanges();

            return result.ToString().TrimEnd();
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