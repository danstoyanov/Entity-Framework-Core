using System;
using System.Text;
using System.Data.SqlClient;

namespace _2._Villain_Names
{
    class Program
    {
        static void Main()
        {
            var connectionStr = "Server=.; Database=MinionsDB; Trusted_Connection=True";
            var connection = new SqlConnection(connectionStr);

            var result = new StringBuilder();

            connection.Open();
            var queryOutput = @"SELECT 
	                                vill.[Name],
	                                COUNT(vill.Id) AS [MinionsCount]
	                                	FROM Villains vill
	                                	JOIN MinionsVillains mv ON mv.VillainId = vill.Id
	                                	JOIN Minions m ON mv.MinionId = m.Id
	                                GROUP BY vill.[Name], vill.Id
	                                HAVING COUNT(vill.[Name]) > 3
	                                ORDER BY COUNT(vill.Id)";

            using (var command = new SqlCommand(queryOutput, connection))
            {

                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var name = dataReader[0];
                        var currCount = dataReader[1];

                        result.AppendLine($"{name} - {currCount}");
                    }
                }
            }

            connection.Close();
            Console.WriteLine(result.ToString());
        }
    }
}
