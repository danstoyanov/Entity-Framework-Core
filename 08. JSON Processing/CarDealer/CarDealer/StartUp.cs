using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using AutoMapper;
using CarDealer.Data;
using CarDealer.Models;
using Microsoft.EntityFrameworkCore;
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

            //  problem 14
            //  Console.WriteLine(GetOrderedCustomers(context));

            //  problem 15
            //  Console.WriteLine(GetCarsFromMakeToyota(context));

            //  problem 16
            //  Console.WriteLine(GetLocalSuppliers(context));

            //  problem 17
            //  Console.WriteLine(GetCarsWithTheirListOfParts(context));

            //  problem 18
            //  Console.WriteLine(GetTotalSalesByCustomer(context));

            //  problem 19
            //  Console.WriteLine(GetSalesWithAppliedDiscount(context));
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
        public static string ImportCars(CarDealerContext context, string inputJson)
        {
            var result = new StringBuilder();
            var inputCarsData = JsonConvert.DeserializeObject<List<ImportCarDto>>(inputJson)
                .ToList();

            var cars = new List<Car>();

            foreach (var car in inputCarsData)
            {
                var currentCar = new Car
                {
                    Make = car.Make,
                    Model = car.Model,
                    TravelledDistance = car.TravelledDistance
                };

                foreach (var part in car.PartCars)
                {
                    PartCar partCar = new PartCar
                    {
                        Car = car,
                        Part = part.Part
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.AddRange(cars);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {cars.Count()}.");
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
            var currOrderedCustomers = context.Customers
                .OrderBy(c => c.BirthDate)
                .ThenBy(c => c.IsYoungDriver)
                .Select(c => new
                {
                    Name = c.Name,
                    BirthDate = c.BirthDate.ToString("dd/MM/yyyy"),
                    IsYoungDriver = c.IsYoungDriver
                })
                .ToList();

            var json = JsonConvert.SerializeObject(currOrderedCustomers, Formatting.Indented);
            return json;
        }
        public static string GetCarsFromMakeToyota(CarDealerContext context)
        {
            var currToyotaCars = context.Cars
                .Where(c => c.Make == "Toyota")
                .Select(c => new
                {
                    Id = c.Id,
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToList();

            var json = JsonConvert.SerializeObject(currToyotaCars, Formatting.Indented);
            return json;
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var currSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(currSuppliers, Formatting.Indented);
            return json;
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var carsWithParts = context.Cars
                .Select(c => new
                {
                    car = new
                    {
                        c.Make,
                        c.Model,
                        c.TravelledDistance
                    },
                    parts = c.PartCars.Select(p => new
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price.ToString("F2"),
                    })
                    .ToList()
                })
                .ToList();

            var json = JsonConvert.SerializeObject(carsWithParts, Formatting.Indented);
            return json;
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var totalSales = context.Customers
                .Where(c => c.Sales.Any(s => s.Car != null))
                .Select(cust => new
                {
                    fullName = cust.Name,
                    boughtCars = cust.Sales.Count(),
                    spentMoney = cust.Sales.Sum(sale => sale.Car.PartCars.Sum(part => part.Part.Price)),
                })
                .OrderByDescending(c => c.spentMoney)
                .ThenByDescending(c => c.boughtCars)
                .ToList();

            var result = JsonConvert.SerializeObject(totalSales, Formatting.Indented);
            return result;
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var salesWithDiscount = context.Sales
                .Select(sale => new
                {
                    car = new
                    {
                        sale.Car.Make,
                        sale.Car.Model,
                        sale.Car.TravelledDistance
                    },
                    customerName = sale.Customer.Name,
                    Discount = sale.Discount.ToString("F2"),
                    price = sale.Car.PartCars.Sum(part => part.Part.Price).ToString("F2"),
                    priceWithDiscount = ((sale.Car.PartCars.Sum(part => part.Part.Price)) - (sale.Discount / 100) * (sale.Car.PartCars.Sum(part => part.Part.Price))).ToString("F2")
                })
                .Take(10)
                .ToList();

            var result = JsonConvert.SerializeObject(salesWithDiscount, Formatting.Indented);
            return result;
        }
    }
}