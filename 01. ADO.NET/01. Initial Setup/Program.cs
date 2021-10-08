using System;
using System.Data.SqlClient;

namespace ADO.NET___Exercise
{
    class Program
    {
        static void Main()
        {
            string connectionString = "Server=.; Database=MinionsDB; Trusted_Connection=True";
            var connection = new SqlConnection(connectionString);

            string createTableQuery = @"CREATE TABLE Countries
                                (
                                	Id INT NOT NULL IDENTITY PRIMARY KEY,
                                	[Name] VARCHAR(40) NOT NULL,
                                )
                                
                                CREATE TABLE Towns
                                (
                                	Id INT NOT NULL IDENTITY PRIMARY KEY,
                                	[Name] VARCHAR(30) NOT NULL,
                                	CountryCode INT NOT NULL FOREIGN KEY REFERENCES Countries (Id)
                                )
                                
                                CREATE TABLE Minions
                                (
                                	Id INT NOT NULL IDENTITY PRIMARY KEY,
                                	[Name] VARCHAR(40) NOT NULL,
                                	Age INT NOT NULL,
                                	TownId INT NOT NULL FOREIGN KEY REFERENCES Towns (Id)
                                )
                                
                                CREATE TABLE EvilnessFactors
                                (
                                	Id INT NOT NULL IDENTITY PRIMARY KEY,
                                	[Name] VARCHAR(40) NOT NULL,
                                )
                                
                                CREATE TABLE Villains
                                (
                                	Id INT NOT NULL IDENTITY PRIMARY KEY,
                                	[Name] VARCHAR(40) NOT NULL,
                                	EvilnessFactorId INT NOT NULL FOREIGN KEY REFERENCES EvilnessFactors (Id)
                                )
                                
                                CREATE TABLE MinionsVillains
                                (
                                	MinionId INT NOT NULL FOREIGN KEY REFERENCES Minions (Id),
                                	VillainId INT NOT NULL FOREIGN KEY  REFERENCES Villains (Id),
                                	PRIMARY KEY (MinionId, VillainId)
                                )";

            var cmd = new SqlCommand(createTableQuery, connection);

            try
            {
                connection.Open();
                Console.WriteLine("Welcome !");

                cmd.ExecuteNonQuery();
                Console.WriteLine("The tables was created successfully");
            }
            catch (SqlException exSql )
            {
                Console.WriteLine($"Error => {exSql.ToString()}");
            }
            finally
            {
                connection.Close();
                Console.WriteLine("Exit from Database !");
            }
        }
    }
}
