namespace TeisterMask.DataProcessor
{
    using System;
    using System.Collections.Generic;

    using Data;
    using System.Text;
    using System.ComponentModel.DataAnnotations;

    using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
    using Newtonsoft.Json;
    using TeisterMask.DataProcessor.ImportDto;
    using TeisterMask.Data.Models;
    using System.Xml.Serialization;
    using System.IO;
    using System.Linq;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfullyImportedProject
            = "Successfully imported project - {0} with {1} tasks.";

        private const string SuccessfullyImportedEmployee
            = "Successfully imported employee - {0} with {1} tasks.";

        public static string ImportProjects(TeisterMaskContext context, string xmlString)
        {
            var result = new StringBuilder();

            var xmlSerializer = new XmlSerializer(typeof(XmlProjectModel[]), new XmlRootAttribute("Projects"));
            var xmlProjects = (XmlProjectModel[])xmlSerializer.Deserialize(new StringReader(xmlString));

            foreach (var xmlProject in xmlProjects)
            {
                if (!IsValid(xmlProject.OpenDate) || !IsValid(xmlProject.Name))
                {
                    result.AppendLine("Invalid data!");
                    continue;
                }

                // check tasks - dates = 0;
                var currTasks = context.Tasks.Where(t => t.Project.Name == xmlProject.Name);

                // later 
                var project = new Project()
                {
                    Name = xmlProject.Name,
                    OpenDate = xmlProject.OpenDate,
                    DueDate = xmlProject.DueDate
                };
            }


            return result.ToString().TrimEnd();
        }

        public static string ImportEmployees(TeisterMaskContext context, string jsonString)
        {
            var result = new StringBuilder();

            var employees = JsonConvert.DeserializeObject<IEnumerable<ImportEmployeesDto>>(jsonString);

            foreach (var employee in employees)
            {
                if ((!IsValid(employee.Username)) || (!IsValid(employee.Email) || (!IsValid(employee.Phone))))
                {
                    result.AppendLine("Invalid data!");
                    continue;
                }


                var currEmployee = new Employee()
                {
                    Username = employee.Username,
                    Email = employee.Email,
                    Phone = employee.Phone,
                };


                ;
            }

            context.SaveChanges();

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