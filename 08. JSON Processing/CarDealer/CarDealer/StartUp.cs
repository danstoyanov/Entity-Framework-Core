using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Newtonsoft.Json;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            //  context.Database.EnsureDeleted();
            //  context.Database.EnsureCreated();

            //  problem 9
            //  var inputSuppliers = File.ReadAllText("../../../Datasets/suppliers.json");
            //  Console.WriteLine(ImportSuppliers(context, inputSuppliers));

            //  problem 10
            //  var inputParts = File.ReadAllText("../../../Datasets/parts.json");
            //  Console.WriteLine(ImportParts(context, inputParts));

            //  problem 11 => NOT FINISHED !!!
            //  var inputCars = File.ReadAllText("../../../Datasets/cars.json");
            //  Console.WriteLine(ImportCars(context, inputCars));

            //  problem 12
            //  var inputCustomers = File.ReadAllText("../../../Datasets/customers.json");
            //  Console.WriteLine(ImportCustomers(context, inputCustomers));

            //  problem 13 => NOT FINISHED !!!
            //  var inputSales = File.ReadAllText("../../../Datasets/sales.json");
            //  Console.WriteLine(ImportSales(context, inputSales));
        }

        public static string ImportSuppliers(CarDealerContext context, string inputJson)
        {
            var result = new StringBuilder();
            var inputSuppliersData = JsonConvert.DeserializeObject<List<Supplier>>(inputJson);

            context.AddRange(inputSuppliersData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputSuppliersData.Count()}.");
            return result.ToString().TrimEnd();
        }
        public static string ImportParts(CarDealerContext context, string inputJson)
        {
            var result = new StringBuilder();

            var checkPartsData = JsonConvert.DeserializeObject<List<Part>>(inputJson);
            var inputParsData = new List<Part>();

            var currSupliers = context.Suppliers.ToList();

            foreach (var part in checkPartsData)
            {
                var currId = currSupliers.FindIndex(supp => supp.Id == part.SupplierId);

                if (currId > 0)
                {
                    inputParsData.Add(part);
                }
            }

            context.AddRange(inputParsData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputParsData.Count()}.");
            return result.ToString().TrimEnd();
        }

        // Not finished !!! 
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var result = new StringBuilder();
            var inputCarsData = JsonConvert.DeserializeObject<List<Car>>(inputJson);

            context.AddRange(inputCarsData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputCarsData.Count()}.");
            return result.ToString().TrimEnd();
        }
        public static string ImportCustomers(CarDealerContext context, string inputJson)
        {
            var result = new StringBuilder();
            var inputCustomersData = JsonConvert.DeserializeObject<List<Customer>>(inputJson);

            context.AddRange(inputCustomersData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputCustomersData.Count()}.");
            return result.ToString().TrimEnd();
        }

        // Not finished !!! 
        public static string ImportSales(CarDealerContext context, string inputJson)
        {
            var result = new StringBuilder();
            var inputSalesdata = JsonConvert.DeserializeObject<List<Sale>>(inputJson);

            context.AddRange(inputSalesdata);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputSalesdata.Count()}.");
            return result.ToString();
        }
        public static string GetOrderedCustomers(CarDealerContext context)
        {
            return "";
        }

    }
}