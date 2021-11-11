using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        public static void Main()
        {
            var context = new ProductShopContext();

            // context.Database.EnsureDeleted();
            // context.Database.EnsureCreated();

            ////  problem 2
            //var currUsers = File.ReadAllText("../../../Datasets/users.json");
            //Console.WriteLine(ImportUsers(context, currUsers));
            //
            ////  problem 3
            //var inputProducts = File.ReadAllText("../../../Datasets/products.json");
            //Console.WriteLine(ImportProducts(context, inputProducts));
            //
            ////  problem 4
            //var inputCategories = File.ReadAllText("../../../Datasets/categories.json");
            //Console.WriteLine(ImportCategories(context, inputCategories));
            //
            ////  problem 5
            //var inputCategoriesProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            //Console.WriteLine(ImportCategoryProducts(context, inputCategoriesProducts));

            ////  problem 6
            //  Console.WriteLine(GetProductsInRange(context));

            ////  problem 7
            //  Console.WriteLine(GetSoldProducts(context));

            ////  problem 8
            //  Console.WriteLine(GetCategoriesByProductsCount(context));

            ////  problem 9 => NOT FINISHED !!!
            //Console.WriteLine(GetUsersWithProducts(context));
        }

        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            var result = new StringBuilder();
            var inputUsersData = JsonConvert.DeserializeObject<List<User>>(inputJson);

            context.AddRange(inputUsersData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputUsersData.Count()}");
            return result.ToString().TrimEnd();
        }
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            var result = new StringBuilder();

            var inputProductsData = JsonConvert.DeserializeObject<List<Product>>(inputJson);
            context.AddRange(inputProductsData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputProductsData.Count()}");
            return result.ToString().TrimEnd();
        }
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {
            var result = new StringBuilder();

            var inputCategoriesData = JsonConvert.DeserializeObject<List<Category>>(inputJson);
            var categoryesData = new List<Category>();

            foreach (var category in inputCategoriesData)
            {
                if (category.Name != null)
                {
                    categoryesData.Add(category);
                }
            }

            context.AddRange(categoryesData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {categoryesData.Count()}");
            return result.ToString().TrimEnd();
        }
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            var result = new StringBuilder();

            var inputCategoryProductsData = JsonConvert.DeserializeObject<List<CategoryProduct>>(inputJson);

            context.AddRange(inputCategoryProductsData);
            context.SaveChanges();

            result.AppendLine($"Successfully imported {inputCategoryProductsData.Count()}");
            return result.ToString().TrimEnd();
        }
        public static string GetProductsInRange(ProductShopContext context)
        {
            var currProducts = context.Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .OrderBy(p => p.price)
                .ToList();

            var json = JsonConvert.SerializeObject(currProducts, Formatting.Indented);

            return json;
        }
        public static string GetSoldProducts(ProductShopContext context)
        {
            var userSoldProd = context.Users
                .Where(p => p.ProductsSold.Any(b => b.Buyer != null))
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                                    .Where(b => b.Buyer != null)
                                    .Select(p => new
                                    {
                                        name = p.Name,
                                        price = p.Price,
                                        buyerFirstName = p.Buyer.FirstName,
                                        buyerLastName = p.Buyer.LastName
                                    })
                                    .ToList()
                })
                .OrderBy(p => p.lastName)
                .ThenBy(p => p.firstName)
                .ToList();

            var json = JsonConvert.SerializeObject(userSoldProd, Formatting.Indented);

            return json;
        }
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var currCategories = context.Categories
                .Select(c => new
                {
                    category = c.Name,
                    productsCount = c.CategoryProducts.Count(),
                    averagePrice = c.CategoryProducts.Average(p => p.Product.Price).ToString("F2"),
                    totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price).ToString()
                })
                .OrderByDescending(c => c.productsCount)
                .ToList();

            var json = JsonConvert.SerializeObject(currCategories, Formatting.Indented);

            return json;
        }
        public static string GetUsersWithProducts(ProductShopContext context)
        {
          // var currUsersWithProd = context.Users
          //     .Where(userProd => userProd.ProductsSold.Any(prod => prod.Buyer != null))
          //     .Include(c => c.ProductsSold)
          //     .OrderByDescending(usersProd => usersProd.ProductsSold.Count(p => p.Buyer != null))
          //     .ToList()
          //     .Select(u => new
          //     {
          //         lastName = u.LastName,
          //         age = u.Age,
          //         soldProducts = new
          //         {
          //             count = u.ProductsSold.Count(p => p.Buyer != null),
          //             products = u.ProductsSold
          //             .ToList()
          //             .Where(p => p.Buyer != null)
          //             .Select(p => new
          //             {
          //                 name = p.Name,
          //                 price = p.Price
          //             })
          //             .ToList()
          //         }
          //     })
          //     .ToList();
          //
          // var result = new
          // {
          //     usersCount = currUsersWithProd.Count,
          //     users = currUsersWithProd
          // };
          //
          // var json = JsonConvert.SerializeObject(result, new JsonSerializerSettings
          // {
          //     Formatting = Formatting.Indented,
          //     NullValueHandling = NullValueHandling.Ignore
          // });

            return "";
        }
    }
}