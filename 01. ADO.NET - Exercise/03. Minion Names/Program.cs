using System;
using System.Text;
using System.Data.SqlClient;

namespace _03._Minion_Names
{
    public class Program
    {
        /* 
         * This problems is not ready yet !!!
        */  

        const string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True";

        static void Main()
        {
            var result = new StringBuilder();
            var connect = new SqlConnection(connectionString);

            connect.Open();

            Console.WriteLine("Enter the Villain Id:");
            var inputId = int.Parse(Console.ReadLine());

            var villianQuery = $@"SELECT TOP (1) v.[Name] AS [Name]
	                            FROM Villains v
	                            JOIN MinionsVillains mv ON mv.VillainId = v.Id
	                            WHERE VillainId = {inputId}";
            VillainsQuery(result, connect, villianQuery);

            var minionsQuery = $@"SELECT 
	                        m.[Name] AS [NAME],
	                        m.Age AS [Age]
	                        	FROM Minions m
	                        	JOIN MinionsVillains mv ON mv.MinionId = m.Id
	                        	WHERE VillainId = {inputId}";
            MinionsQuery(result, connect, minionsQuery);

            connect.Close();
            Console.WriteLine(result.ToString());
        }

        private static void VillainsQuery(StringBuilder result, SqlConnection connect, string villianQuery)
        {
            using (var command = new SqlCommand(villianQuery, connect))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    while (dataReader.Read())
                    {
                        var villianName = dataReader[0];

                        result.AppendLine($"Villain: {villianName}");
                    }
                }
            }
        }

        private static void MinionsQuery(StringBuilder result, SqlConnection connect, string minionsQuery)
        {
            using (var command = new SqlCommand(minionsQuery, connect))
            {
                using (var dataReader = command.ExecuteReader())
                {
                    var count = 1;

                    while (dataReader.Read())
                    {
                        var minionName = dataReader[0];
                        var minionAge = dataReader[1];

                        result.AppendLine($"{count++}. {minionName} {minionAge}");
                    }
                }
            }
        }
    }
}
