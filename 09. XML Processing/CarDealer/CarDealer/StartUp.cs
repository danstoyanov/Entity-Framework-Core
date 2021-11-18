using System;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using System.Collections.Generic;

using CarDealer.Data;
using CarDealer.Models;
using CarDealer.Dto.Imput;
using CarDealer.Dto.Export;
using CarDealer.Dto.Import;
using CarDealer.XMLConverter;
using Microsoft.EntityFrameworkCore;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new CarDealerContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();
            //
            ////  problem 1
            //var suppliersRead = File.ReadAllText("../../../Datasets/suppliers.xml");
            //Console.WriteLine(ImportSuppliers(context, suppliersRead));
            ////  problem 2
            //var partsRead = File.ReadAllText("../../../Datasets/parts.xml");
            //Console.WriteLine(ImportParts(context, partsRead));
            ////  problem 3 
            //var carsRead = File.ReadAllText("../../../Datasets/cars.xml");
            //Console.WriteLine(ImportCars(context, carsRead));
            ////  problem 4 
            //var customersRead = File.ReadAllText("../../../Datasets/customers.xml");
            //Console.WriteLine(ImportCustomers(context, customersRead));
            ////  problem 5
            //var salesRead = File.ReadAllText("../../../Datasets/sales.xml");
            //Console.WriteLine(ImportSales(context, salesRead));
            ////  problem 6
            //Console.WriteLine(GetCarsWithDistance(context));
            ////  problem 7
            //Console.WriteLine(GetCarsFromMakeBmw(context));
            ////  problem 8
            //Console.WriteLine(GetLocalSuppliers(context));
            ////  problem 9
            //Console.WriteLine(GetCarsWithTheirListOfParts(context));
            //  problem 10
            //Console.WriteLine(GetTotalSalesByCustomer(context));
            //  problem 11
            //Console.WriteLine(GetSalesWithAppliedDiscount(context));
        }
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            var suppliers = XmlConverter.Deserializer<ImportSupplierDto>(inputXml, "Suppliers");
            var currSuppliers = new List<Supplier>();

            foreach (var supplier in suppliers)
            {
                var currSupplier = new Supplier()
                {
                    Name = supplier.Name,
                    IsImporter = supplier.IsImporter
                };

                currSuppliers.Add(currSupplier);
            }

            context.Suppliers.AddRange(currSuppliers);
            context.SaveChanges();

            return $"Successfully imported {currSuppliers.Count}";
        }
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            var parts = XmlConverter.Deserializer<ImportPartDto>(inputXml, "Parts");

            var currParts = new List<Part>();
            var currSuppliers = context.Suppliers.Select(s => s.Id).ToArray();

            foreach (var part in parts)
            {
                var currParSuppId = part.SupplierId;

                if (!currSuppliers.Contains(currParSuppId))
                {
                    continue;
                }

                var currPart = new Part()
                {
                    Name = part.Name,
                    Price = part.Price,
                    Quantity = part.Quantity,
                    SupplierId = part.SupplierId
                };

                currParts.Add(currPart);
            }

            context.Parts.AddRange(currParts);
            context.SaveChanges();

            return $"Successfully imported {currParts.Count}";
        }
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            var carsDtos = XmlConverter.Deserializer<ImportCarDto>(inputXml, "Cars");
            var cars = new List<Car>();

            var allParts = context.Parts.Select(p => p.Id).ToList();

            foreach (var currCar in carsDtos)
            {
                var distParts = currCar.CarPartsIds.Select(p => p.Id).Distinct();
                var parts = distParts.Intersect(allParts);

                var car = new Car
                {
                    Make = currCar.Make,
                    Model = currCar.Model,
                    TravelledDistance = currCar.TraveledDistance
                };

                foreach (var part in parts)
                {
                    var partCar = new PartCar
                    {
                        PartId = part
                    };

                    car.PartCars.Add(partCar);
                }

                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }
        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            var customers = XmlConverter.Deserializer<ImportCustomerDto>(inputXml, "Customers");
            var currCustomers = new List<Customer>();

            foreach (var customer in customers)
            {
                var currCustomer = new Customer()
                {
                    Name = customer.Name,
                    BirthDate = customer.BirthDate,
                    IsYoungDriver = customer.isYoungDriver
                };

                currCustomers.Add(currCustomer);
            }

            context.Customers.AddRange(currCustomers);
            context.SaveChanges();

            return $"Successfully imported {currCustomers.Count}";
        }
        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            var sales = XmlConverter.Deserializer<ImportSaleDto>(inputXml, "Sales");

            var currSales = new List<Sale>();
            var currCars = context.Cars.Select(c => c.Id).ToArray();

            foreach (var sale in sales)
            {
                var currCarId = sale.CarId;

                if (!currCars.Contains(currCarId))
                {
                    continue;
                }

                var currSale = new Sale()
                {
                    CarId = sale.CarId,
                    CustomerId = sale.CustomerId,
                    Discount = sale.Discount
                };

                currSales.Add(currSale);
            }

            context.Sales.AddRange(currSales);
            context.SaveChanges();

            return $"Successfully imported {currSales.Count}";
        }
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var currCars = context.Cars
                .Where(c => c.TravelledDistance > 2_000_000)
                .Select(c => new ExportCarDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportCarDto[]), new XmlRootAttribute("cars"));
            var result = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlSerializer.Serialize(result, currCars, ns);

            return result.ToString().TrimEnd();
        }
        public static string GetCarsFromMakeBmw(CarDealerContext context)
        {
            var bmwCars = context.Cars
                .Where(c => c.Make == "BMW")
                .Select(c => new ExportBmwCarsDto
                {
                    Id = c.Id,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance
                })
                .OrderBy(c => c.Model)
                .ThenByDescending(c => c.TravelledDistance)
                .ToArray();

            var result = XmlConverter.Serialize(bmwCars, "cars");
            return result.ToString().TrimEnd();
        }
        public static string GetLocalSuppliers(CarDealerContext context)
        {
            var currSuppliers = context.Suppliers
                .Where(s => s.IsImporter == false)
                .Select(s => new ExportSuppliersDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    PartsCount = s.Parts.Count
                })
                .ToArray();

            var result = XmlConverter.Serialize(currSuppliers, "suppliers");
            return result.ToString().TrimEnd();
        }
        public static string GetCarsWithTheirListOfParts(CarDealerContext context)
        {
            var currCarParts = context.Cars
                .Select(c => new ExportCarsPartsDto
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance,
                    PartsCar = c.PartCars.Select(p => new CarPartsExportDto
                    {
                        Name = p.Part.Name,
                        Price = p.Part.Price
                    })
                    .OrderByDescending(p => p.Price)
                    .ToArray()
                })
                .OrderByDescending(c => c.TravelledDistance)
                .ThenBy(c => c.Model)
                .Take(5)
                .ToArray();

            var result = XmlConverter.Serialize(currCarParts, "cars");
            return result.ToString().TrimEnd();
        }
        public static string GetTotalSalesByCustomer(CarDealerContext context)
        {
            var currCustomers = context.Customers
                .Include(c => c.Sales)
                .ThenInclude(c => c.Car)
                .ThenInclude(c => c.PartCars)
                .ThenInclude(c => c.Part)
                .Where(c => c.Sales.Any(c => c.Car != null))
                .ToArray()
                .Select(c => new ExportCustomersDto
                {
                    FullName = c.Name,
                    BoughtCars = c.Sales.Count,
                    SpendMoney = c.Sales.Sum(sale => sale.Car.PartCars.Sum(part => part.Part.Price))
                })
                .OrderByDescending(c => c.SpendMoney)
                .ToArray();

            var result = XmlConverter.Serialize(currCustomers, "customers");
            return result.ToString().TrimEnd();
        }
        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            var currSales = context
                .Sales
                .Select(s => new ExportSalesDto
                {
                    Car = new ExportCarSaleDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance
                    },
                    CustomerName = s.Customer.Name,
                    Discount = s.Discount,
                    Price = s.Car.PartCars.Sum(c => c.Part.Price),
                    PriceWithDiscount = (s.Car.PartCars.Sum(c => c.Part.Price) - (((s.Car.PartCars.Sum(c => c.Part.Price)) * (s.Discount)) / 100))
                })
                .ToArray();

            var xmlSerializer = new XmlSerializer(typeof(ExportSalesDto[]), new XmlRootAttribute("sales"));
            var result = new StringWriter();

            var ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            xmlSerializer.Serialize(result, currSales, ns);
            return result.ToString().TrimEnd();
        }
    }
}