using System;
using Microsoft.Data.SqlClient;

namespace Demo_EF_Core
{
    public class Program
    {
        static void Main()
        {
            var softUniDatabase = "Server=.;Integrated Security=true;Database=SoftUni";
            var testConnection = new SqlConnection(softUniDatabase);

            try
            {
                testConnection.Open();
                Console.WriteLine("Concrats you connect to server !");
                testConnection.Close();
            }
            catch (Exception)
            {
                Console.WriteLine("You FAIL to connect !!!");
            }

        }
    }
}
