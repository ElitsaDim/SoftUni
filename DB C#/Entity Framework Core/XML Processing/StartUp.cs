using CarDealer.Data;
using CarDealer.DTO.ExportDto;
using CarDealer.DTO.ImportDto;
using CarDealer.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace CarDealer
{
    public class StartUp
    {
        public static void Main(string[] args)
        {
            CarDealerContext dbContext = new CarDealerContext();
            //ResetDb(dbContext);

            //string inputXmlSupplier = File.ReadAllText("../../../Datasets/suppliers.xml");
            //string resultSupplier = ImportSuppliers(dbContext, inputXmlSupplier);

            //string inputXmlPart = File.ReadAllText("../../../Datasets/parts.xml");
            //string resultPart = ImportParts(dbContext, inputXmlPart);

            //string inputXmlCar = File.ReadAllText("../../../Datasets/cars.xml");
            //string resultCar = ImportCars(dbContext, inputXmlCar);

            //string inputXml = File.ReadAllText("../../../Datasets/sales.xml");
            string result = GetSalesWithAppliedDiscount(dbContext);
            Console.WriteLine(result);
        }

        private static void ResetDb(CarDealerContext dbContext)
        {
            dbContext.Database.EnsureDeleted();
            dbContext.Database.EnsureCreated();

            Console.WriteLine("Db reset!");
        }

        //Problem 09
        public static string ImportSuppliers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Suppliers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSupplierDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSupplierDto[] dtos = (ImportSupplierDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Supplier> suppliers = new HashSet<Supplier>();

            foreach (var supplierDto in dtos)
            {
                Supplier s = new Supplier()
                {
                    Name = supplierDto.Name,
                    IsImporter = bool.Parse(supplierDto.IsImporter)
                };

                suppliers.Add(s);
            }

            context.Suppliers.AddRange(suppliers);
            context.SaveChanges();

            return $"Successfully imported {suppliers.Count}";
        }

        //Problem 10
        public static string ImportParts(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Parts");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportPartDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportPartDto[] dtos = (ImportPartDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Part> parts = new HashSet<Part>();

            foreach (var dto in dtos)
            {
                Supplier supplier = context
                    .Suppliers
                    .Find(dto.SupplierId);

                if (supplier == null)
                {
                    continue;
                }

                Part part = new Part()
                {
                    Name = dto.Name,
                    Price = decimal.Parse(dto.Price),
                    Quantity = dto.Quantity,
                    SupplierId = dto.SupplierId
                };
                parts.Add(part);
            }

            context.Parts.AddRange(parts);
            context.SaveChanges();

            return $"Successfully imported {parts.Count}";
        }

        //Problem 11
        public static string ImportCars(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Cars");

            XmlSerializer xmlSerialier = new XmlSerializer(typeof(ImportCarDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportCarDto[] dtos = (ImportCarDto[])xmlSerialier.Deserialize(stringReader);

            ICollection<Car> cars = new HashSet<Car>();

            foreach (var dto in dtos)
            {
                Car car = new Car()
                {
                    Make = dto.Make,
                    Model = dto.Model,
                    TravelledDistance = dto.TravelledDistance
                };
                ICollection<PartCar> carParts = new HashSet<PartCar>();

                foreach (int partId in dto.Parts.Select(p => p.Id).Distinct())
                {
                    Part part = context
                        .Parts
                        .Find(partId);

                    if (part == null)
                    {
                        continue;
                    }

                    PartCar partCar = new PartCar()
                    {
                        Car = car,
                        Part = part
                    };
                    carParts.Add(partCar);
                }

                car.PartCars = carParts;
                cars.Add(car);
            }

            context.Cars.AddRange(cars);
            context.SaveChanges();

            return $"Successfully imported {cars.Count}";
        }

        //Problem 12

        public static string ImportCustomers(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Customers");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportCustomersDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportCustomersDto[] dtos = (ImportCustomersDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Customer> customers = new HashSet<Customer>();

            foreach (var dto in dtos)
            {
                Customer customer = new Customer()
                {
                    Name = dto.Name,
                    BirthDate = DateTime.Parse(dto.BirthDate),
                    IsYoungDriver = bool.Parse(dto.IsYoungDriver)
                };

                customers.Add(customer);
            }

            context.Customers.AddRange(customers);
            context.SaveChanges();

            return $"Successfully imported {customers.Count}";
        }

        //Problem 13

        public static string ImportSales(CarDealerContext context, string inputXml)
        {
            XmlRootAttribute xmlRoot = new XmlRootAttribute("Sales");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ImportSaleDto[]), xmlRoot);

            using StringReader stringReader = new StringReader(inputXml);

            ImportSaleDto[] dtos = (ImportSaleDto[])xmlSerializer.Deserialize(stringReader);

            ICollection<Sale> sales = new HashSet<Sale>();

            foreach (var dto in dtos)
            {
                Car car = context
                    .Cars
                    .Find(dto.CarId);

                if (car == null)
                {
                    continue;
                }
                else
                {
                    Sale sale = new Sale()
                    {
                        CarId = dto.CarId,
                        CustomerId = dto.CustomerId,
                        Discount = dto.Discount
                    };

                    sales.Add(sale);
                }
            }

            context.Sales.AddRange(sales);
            context.SaveChanges();

            return $"Successfully imported {sales.Count}";
        }

        //Problem 14
        public static string GetCarsWithDistance(CarDealerContext context)
        {
            var sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute("cars");

            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(String.Empty, String.Empty);

            ExportCarsWithDistanceDto[] carsDtos = context
                .Cars
                .Where(c => c.TravelledDistance > 2000000)
                .OrderBy(c => c.Make)
                .ThenBy(c => c.Model)
                .Take(10)
                .Select(c => new ExportCarsWithDistanceDto()
                {
                    Make = c.Make,
                    Model = c.Model,
                    TravelledDistance = c.TravelledDistance.ToString()
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, carsDtos, namespaces);

            return sb.ToString().TrimEnd();
        }

        //Problem 19

        public static string GetSalesWithAppliedDiscount(CarDealerContext context)
        {
            StringBuilder sb = new StringBuilder();
            using StringWriter stringWriter = new StringWriter(sb);

            XmlRootAttribute xmlRoot = new XmlRootAttribute("sales");
            XmlSerializer xmlSerializer = new XmlSerializer(typeof(ExportSalesWithDiscountDto[]), xmlRoot);

            XmlSerializerNamespaces namespaces = new XmlSerializerNamespaces();
            namespaces.Add(string.Empty, string.Empty);

            ExportSalesWithDiscountDto[] dtos = context
                .Sales
                .Select(s => new ExportSalesWithDiscountDto()
                {
                    Car = new ExportSalesCarDto()
                    {
                        Make = s.Car.Make,
                        Model = s.Car.Model,
                        TravelledDistance = s.Car.TravelledDistance.ToString()
                    },
                    Discount = s.Discount.ToString("f2"),
                    CustomerName = s.Customer.Name,
                    Price = s.Car.PartCars.Sum(pc => pc.Part.Price).ToString("f2"),
                    PriceWithDiscount = (s.Car.PartCars.Sum(pc => pc.Part.Price) - s.Car.PartCars.Sum(pc => pc.Part.Price)
                                        * s.Discount/100)
                                        .ToString("")
                })
                .ToArray();

            xmlSerializer.Serialize(stringWriter, dtos, namespaces);

            return sb.ToString().TrimEnd();
        }
    }

    
}