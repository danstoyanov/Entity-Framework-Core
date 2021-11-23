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