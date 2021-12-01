namespace SoftJail.DataProcessor
{
    using Data;
    using System;
    using System.IO;
    using System.Linq;
    using System.Globalization;
    using System.Xml.Serialization;

    using Newtonsoft.Json;

    using SoftJail.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportPrisonersByCells(SoftJailDbContext context, int[] ids)
        {
            var jsonPrisonersExport = context
                .Prisoners
                .ToArray()
                .Where(p => ids.Contains(p.Id))
                .Select(p => new
                {
                    Id = p.Id,
                    Name = p.FullName,
                    CellNumber = p.Cell.CellNumber,
                    Officers = p.PrisonerOfficers.Select(o => new
                    {
                        OfficerName = o.Officer.FullName,
                        Department = o.Officer.Department.Name,
                    })
                    .OrderBy(o => o.OfficerName)
                    .ToArray(),
                    TotalOfficerSalary = p.PrisonerOfficers.Sum(o => o.Officer.Salary)
                })
                .OrderBy(o => o.Name)
                .ThenBy(o => o.Id);

            return JsonConvert.SerializeObject(jsonPrisonersExport, Formatting.Indented);
        }

        public static string ExportPrisonersInbox(SoftJailDbContext context, string prisonersNames)
        {

            var currPrisonerNames = prisonersNames.Split(",").ToArray();

            var xmlPrisonersData = context
                .Prisoners
                .Where(p => currPrisonerNames.Contains(p.FullName))
                .Select(p => new XmlPrisonerExportModel
                {
                    Id = p.Id,
                    Name = p.FullName,
                    IncarcerationDate = p.IncarcerationDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                    EncryptedMessages = p.Mails.Select(m => new XmlMessageExportModel
                    {
                        Description = new string(m.Description.Reverse().ToArray())
                    })
                    .ToArray(),
                })
                .OrderBy(p => p.Name)
                .ThenBy(p => p.Id)
                .ToList();

            var result = XmlConverter.Serialize(xmlPrisonersData, "Prisoners");

            return result;
        }
    }
}