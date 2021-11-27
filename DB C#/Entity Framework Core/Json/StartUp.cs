﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using ProductShop.Data;
using ProductShop.Models;

namespace ProductShop
{
    public class StartUp
    {
        private static string ResultDirectoryPath = "../../../Datasets/Results";


        public static void Main(string[] args)
        {
            var context = new ProductShopContext();
            //context.Database.EnsureDeleted();
            //context.Database.EnsureCreated();

            string usersJsonAsString = File.ReadAllText("../../../Datasets/users.json");
            string productsJsonAsString = File.ReadAllText("../../../Datasets/products.json");
            string categoriesJsonAsString = File.ReadAllText("../../../Datasets/categories.json");
            string categoryProductJsonAsString = File.ReadAllText("../../../Datasets/categories-products.json");


           
            //ImportUsers(context, usersJsonAsString);
            //ImportProducts(context, productsJsonAsString);
            //ImportCategories(context, categoriesJsonAsString);
            //ImportCategoryProducts(context, categoryProductJsonAsString);
            string json = GetProductsInRange(context);
            string jsonProducts = GetSoldProducts(context);
            string jsonCategories = GetCategoriesByProductsCount(context);
            string jsonUsers = GetUsersWithProducts(context);
            string jsonUUsersAndProducts = GetUsersWithProducts(context);

            if (!Directory.Exists(ResultDirectoryPath))
            {
                Directory.CreateDirectory(ResultDirectoryPath);
            }

            File.WriteAllText("../../../Datasets/Results/users-and-products.json", jsonUUsersAndProducts);

        }

        //Problem 01
        public static string ImportUsers(ProductShopContext context, string inputJson)
        {
            User[] users = JsonConvert.DeserializeObject<User[]>(inputJson);

            context.Users.AddRange(users);
            context.SaveChanges();

            return $"Successfully imported {users.Length}";
        }

        //Problem 02
        public static string ImportProducts(ProductShopContext context, string inputJson)
        {
            Product[] products = JsonConvert.DeserializeObject<Product[]>(inputJson);

            context.Products.AddRange(products);
            context.SaveChanges();

            return $"Successfully imported {products.Length}";
        }

        //Problem 03
        public static string ImportCategories(ProductShopContext context, string inputJson)
        {

            Category[] categories = JsonConvert.DeserializeObject<Category[]>(inputJson)
                .Where(x => x.Name != null)
                .ToArray();

            context.Categories.AddRange(categories);
            context.SaveChanges();

            return $"Successfully imported {categories.Length}";
        }

        //Problem 04
        public static string ImportCategoryProducts(ProductShopContext context, string inputJson)
        {
            CategoryProduct[] categoryProducts = JsonConvert.DeserializeObject<CategoryProduct[]>(inputJson);

            context.CategoryProducts.AddRange(categoryProducts);
            context.SaveChanges();

            return $"Successfully imported {categoryProducts.Length}";
        }

        //Problem 05
        public static string GetProductsInRange(ProductShopContext context)
        {
            var products = context
                .Products
                .Where(p => p.Price >= 500 && p.Price <= 1000)
                .OrderBy(p => p.Price)
                .Select(p => new
                {
                    name = p.Name,
                    price = p.Price,
                    seller = p.Seller.FirstName + " " + p.Seller.LastName
                })
                .ToArray();

            string json = JsonConvert.SerializeObject(products, Formatting.Indented);

            return json;
        }

        //Problem 06
        public static string GetSoldProducts(ProductShopContext context)
        {
            var products = context
                .Users
                .Where(u => u.ProductsSold.Any(p=>p.Buyer!= null))
                .OrderBy(u => u.LastName)
                .ThenBy(u => u.FirstName)
                .Select(u => new
                {
                    firstName = u.FirstName,
                    lastName = u.LastName,
                    soldProducts = u.ProductsSold
                        .Where(p => p.Buyer != null)
                        .Select(p => new
                        {
                            name = p.Name,
                            price = p.Price,
                            buyerFirstName = p.Buyer.FirstName,
                            buyerLastName = p.Buyer.LastName
                        })
                })
                .ToArray();

            string jsonProducts = JsonConvert.SerializeObject(products, Formatting.Indented);

            return jsonProducts;
        }

        //Problem 07
        public static string GetCategoriesByProductsCount(ProductShopContext context)
        {
            var cateories = context
                .Categories
                .Select(c => new
                {
                    name = c.Name,
                    productsCount = c.CategoryProducts.Count,
                    averagePrice = c.CategoryProducts.Average(p => p.Product.Price).ToString("f2"),
                    totalRevenue = c.CategoryProducts.Sum(p => p.Product.Price).ToString("f2")
                })
                .OrderByDescending(c => c.productsCount)
                .ToArray();

            string json = JsonConvert.SerializeObject(cateories, Formatting.Indented);

            return json;
        }

        //Problem 08
        public static string GetUsersWithProducts(ProductShopContext context)
        {
            var users = context
                .Users
                .Where(u => u.ProductsSold.Any(p => p.Buyer != null))
                .Select(u => new
                {
                    lastName = u.LastName,
                    age = u.Age,
                    soldProducts = new
                    {
                        count = u.ProductsSold.Count(p => p.Buyer != null),
                        products = u.ProductsSold
                                .Where(p => p.Buyer != null)
                                .Select(p => new
                                {
                                    name = p.Name,
                                    price = p.Price
                                })
                                .ToArray()
                    }
                })
                .OrderByDescending(u => u.soldProducts.count)
                .ToArray();

            var resultObject = new
            {
                usersCount = users.Length,
                users = users
            };

            var settins = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                Formatting = Formatting.Indented
            };


            string json = JsonConvert.SerializeObject(resultObject, settins);

            return json;
        }

    }
}