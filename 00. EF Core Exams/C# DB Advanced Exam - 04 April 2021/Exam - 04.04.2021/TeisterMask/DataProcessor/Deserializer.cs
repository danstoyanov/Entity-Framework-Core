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
    using System.Globalization;
    using TeisterMask.Data.Models.Enums;

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
                if (!IsValid(xmlProject))
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                // we must convert the dates duo and open dates !!!!!
                //????????
                //  Parsed Open Date
                DateTime projectOpenDate;
                bool isProjectOpenDate = DateTime.TryParseExact(xmlProject.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectOpenDate);

                //  Parsed Duo Date
                DateTime projectDuoDate;
                bool isProjectDuoDate = DateTime.TryParseExact(xmlProject.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out projectDuoDate);

                if (xmlProject.DueDate == null || xmlProject.DueDate == string.Empty)
                {
                    xmlProject.DueDate = null;
                }

                if (!isProjectOpenDate)
                {
                    result.AppendLine(ErrorMessage);
                    continue;
                }

                var project = new Project()
                {
                    Name = xmlProject.Name,
                    OpenDate = projectOpenDate,
                    DueDate = projectDuoDate,
                };

                foreach (var xmlTask in xmlProject.Tasks)
                {
                    if (!IsValid(xmlTask))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    // we must convert task open date 
                    DateTime taskOpenDate;
                    bool isTaskOpenDate = DateTime.TryParseExact(xmlTask.OpenDate, "dd/MM/yyyy", CultureInfo.InvariantCulture,
                        DateTimeStyles.None, out taskOpenDate);

                    // we must convert task duo date
                    DateTime taskDuoDate;
                    bool isTaskDuoDate = DateTime.TryParseExact(xmlTask.DueDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out taskDuoDate);

                    if (!isProjectOpenDate || !isProjectDuoDate)
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    if ((taskOpenDate < projectOpenDate) || (taskDuoDate > projectDuoDate))
                    {
                        result.AppendLine(ErrorMessage);
                        continue;
                    }

                    // we must convert the enums !!!
                    ExecutionType executionType;
                    bool isExecutionTypeParsed = Enum.TryParse<ExecutionType>(xmlTask.ExecutionType, out executionType);



                    LabelType labelType;
                    
                    var task = new Task()
                    {
                        Name = xmlTask.Name,
                        OpenDate = taskOpenDate,
                        DueDate = taskDuoDate,
                        ExecutionType = xmlTask.ExecutionType.Value,
                        LabelType = xmlTask.LabelType.Value
                    };

                    project.Tasks.Add(task);
                }

                context.Projects.Add(project);
                result.AppendLine($"Successfully imported project - {project.Name} with {project.Tasks.Count()} tasks.");
            }

            context.SaveChanges();
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