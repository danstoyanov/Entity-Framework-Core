using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
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

            context.Database.EnsureDeleted();
            context.Database.EnsureCreated();

            //  problem 2
            //  var currUsers = File.ReadAllText("../../../Datasets/users.json");
            //  Console.WriteLine(ImportUsers(context, currUsers));

            //  problem 3
            //  var inputProducts = File.ReadAllText("../../../Datasets/products.json");
            //  Console.WriteLine(ImportProducts(context, inputProducts));

            //  problem 4
            //  var inputCategories = File.ReadAllText("../../../Datasets/categories.json");
            //  Console.WriteLine(ImportCategories(context, inputCategories));

            //  problem 5
            //  var inputCategoriesProducts = File.ReadAllText("../../../Datasets/categories-products.json");
            //  Console.WriteLine(ImportCategoryProducts(context, inputCategoriesProducts));
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

        // More Exercises !!!
    }
}