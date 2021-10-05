using System;
using System.Data.SqlClient;

namespace Demo___Insert_into_tables
{
    class Program
    {
        static void Main()
        {
            var connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True";
            var connection = new SqlConnection(connectionString);

            var insertTableQuery = @"INSERT INTO Countries ([Name]) VALUES ('Bulgaria'),('England'),('Cyprus'),('Germany'),('Norway')

INSERT INTO Towns ([Name], CountryCode) VALUES ('Plovdiv', 1),('Varna', 1),('Burgas', 1),('Sofia', 1),('London', 2),('Southampton', 2),('Bath', 2),('Liverpool', 2),('Berlin', 3),('Frankfurt', 3),('Oslo', 4)

INSERT INTO Minions (Name,Age, TownId) VALUES('Bob', 42, 3),('Kevin', 1, 1),('Bob ', 32, 6),('Simon', 45, 3),('Cathleen', 11, 2),('Carry ', 50, 10),('Becky', 125, 5),('Mars', 21, 1),('Misho', 5, 10),('Zoe', 125, 5),('Json', 21, 1)

INSERT INTO EvilnessFactors (Name) VALUES ('Super good'),('Good'),('Bad'), ('Evil'),('Super evil')

INSERT INTO Villains (Name, EvilnessFactorId) VALUES ('Gru',2),('Victor',1),('Jilly',3),('Miro',4),('Rosen',5),('Dimityr',1),('Dobromir',2)

INSERT INTO MinionsVillains (MinionId, VillainId) VALUES (4,2),(1,1),(5,7),(3,5),(2,6),(11,5),(8,4),(9,7),(7,1),(1,3),(7,3),(5,3),(4,3),(1,2),(2,1),(2,7)";

            var cmd = new SqlCommand(insertTableQuery, connection);

            try
            {
                connection.Open();
                cmd.ExecuteNonQuery();
                Console.WriteLine("You fill the tables !!");
            }
            catch (SqlException ex)
            {
                Console.WriteLine($"Error message: {ex.ToString()}");
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Exit from Database !");
            }
        }
    }
}
