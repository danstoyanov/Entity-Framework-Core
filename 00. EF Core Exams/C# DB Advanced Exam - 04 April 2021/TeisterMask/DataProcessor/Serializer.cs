namespace TeisterMask.DataProcessor
{
    using System;
    using System.Linq;
    using System.Text;
    using Data;
    using Newtonsoft.Json;
    using Formatting = Newtonsoft.Json.Formatting;

    public class Serializer
    {
        public static string ExportProjectWithTheirTasks(TeisterMaskContext context)
        {
            var result = new StringBuilder();
            result.AppendLine("THE CURRENT EXPORT PROJ IS NOT READY YET !!!");


            return result.ToString().TrimEnd();
        }

        public static string ExportMostBusiestEmployees(TeisterMaskContext context, DateTime date)
        {
            var employeesData = context.Employees.Select(e => e.Username)
                .ToList();


            return JsonConvert.SerializeObject(employeesData);
        }
    }
}