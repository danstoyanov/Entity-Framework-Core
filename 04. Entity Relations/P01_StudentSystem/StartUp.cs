using P01_StudentSystem.Data;
using P01_StudentSystem.Data.Models;
using System;

namespace P01_StudentSystem
{
    public class StartUp
    {
        public static void Main()
        {
            // test insert in database 
            // migrations in EF Core !
            // 
            // 
            // 
            
            var db = new StudentSystemContext();
            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();

            for (int i = 0; i < 5; i++)
            {
                db.Students.Add(new Student
                {
                    Name = "Daniel" + i,
                    PhoneNumber = "033325252" + i,
                    RegisteredOn = DateTime.UtcNow,
                    Birthday = new DateTime(1990 + i, 1, 1)
                });
            }
            db.SaveChanges();
        }
    }
}
