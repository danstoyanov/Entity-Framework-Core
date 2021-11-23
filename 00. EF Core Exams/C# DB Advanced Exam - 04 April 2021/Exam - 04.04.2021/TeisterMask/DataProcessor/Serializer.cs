namespace TeisterMask.DataProcessor
{
    using System;
    using System.Text;
    using Data;

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
            var result = new StringBuilder();
            result.AppendLine("THE CURRENT EXPORT EMPLOYEE IS NOT READY YET !!!");


            return result.ToString().TrimEnd();
        }
    }
}