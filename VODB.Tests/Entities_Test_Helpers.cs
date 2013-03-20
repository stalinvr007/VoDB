using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VODB.Infrastructure;
using VODB.Tests.Models.Northwind;

namespace VODB.Tests
{
    public static class ExistingEntities_Test_Helpers
    {
        /// <summary>
        /// Creates an existing test entity.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static Object CreateExistingTestEntity(this ITable table)
        {

            var entity = Activator.CreateInstance(table.EntityType);

            if (table.EntityType == typeof(Employee))
            {
                return FillEmployee(entity as Employee);
            }
            else if (table.EntityType == typeof(Customers))
            {
                return FillCustomer(entity as Customers);
            }
            else if (table.EntityType == typeof(Categories))
            {
                return FillCategory(entity as Categories);
            }
            else if (table.EntityType == typeof(EmployeeTerritories))
            {
                return FillEmployeeTerritory(entity as EmployeeTerritories);
            }
            else if (table.EntityType == typeof(OrderDetails))
            {
                return FillOrderDetails(entity as OrderDetails);
            }
            else if (table.EntityType == typeof(Orders))
            {
                return FillOrder(entity as Orders);
            }
            else if (table.EntityType == typeof(Products))
            {
                return FillProduct(entity as Products);
            }
            else if (table.EntityType == typeof(Region))
            {
                return FillRegion(entity as Region);
            }
            else if (table.EntityType == typeof(Shippers))
            {
                return FillShipper(entity as Shippers);
            }
            else if (table.EntityType == typeof(Suppliers))
            {
                return FillSupplier(entity as Suppliers);
            }
            else if (table.EntityType == typeof(Territories))
            {
                return FillTerritory(entity as Territories);
            }


            return entity;
        }

        private static object FillTerritory(Territories territories)
        {
            territories.TerritoryId = "01581";
            territories.TerritoryDescription = "Westboro";
            territories.Region = new Region { Id = 1 };
            return territories;
        }

        private static object FillSupplier(Suppliers suppliers)
        {
            suppliers.SupplierId = 1;
            suppliers.CompanyName = "Exotic Liquids";
            return suppliers;
        }

        private static object FillShipper(Shippers shippers)
        {
            shippers.ShipperId = 1;
            shippers.CompanyName = "Speedy Express";
            shippers.Phone = "(503) 555-9831";
            return shippers;
        }

        private static object FillRegion(Region region)
        {
            region.Id = 1;
            region.Description = "Eastern";
            return region;
        }

        private static object FillProduct(Products products)
        {
            products.ProductId = 11;
            products.ProductName = "testing";
            return products;
        }

        private static object FillOrder(Orders orders)
        {
            orders.OrderId = 10248;
            orders.OrderDate = new DateTime(2000, 10, 3);
            orders.RequiredDate = new DateTime(2000, 10, 3);
            orders.ShippedDate = new DateTime(2000, 10, 3);
            return orders;
        }

        private static object FillOrderDetails(OrderDetails orderDetails)
        {
            orderDetails.Order = new Orders { OrderId = 10248 };
            orderDetails.Product = new Products { ProductId = 11 };
            orderDetails.Quantity = 1000;
            return orderDetails;
        }

        private static object FillEmployeeTerritory(EmployeeTerritories employeeTerritories)
        {
            employeeTerritories.Employee = new Employee { EmployeeId = 1 };
            employeeTerritories.Territories = new Territories { TerritoryId = "19713" };
            return employeeTerritories;
        }

        private static object FillCategory(Categories categories)
        {
            categories.CategoryId = 1;
            categories.CategoryName = "Condiments";
            categories.Description = "Sweet and savory sauces, relishes, spreads, and seasonings...";
            return categories;
        }

        private static object FillCustomer(Customers customers)
        {
            customers.CustomerId = "ANATR";
            customers.CompanyName = "Ana Trujillo Emparedados y helados";
            customers.ContactName = "Ana Trujillo";
            customers.ContactTitle = "Owner";
            return customers;
        }

        private static Object FillEmployee(Employee employee)
        {

            employee.EmployeeId = 1;
            employee.FirstName = "test name";
            employee.LastName = "test last name";
            employee.BirthDate = new DateTime(1983, 4, 16);
            employee.HireDate = new DateTime(1999, 4, 16);
            employee.Photo = new Byte[10];

            return employee;
        }

    }

    public static class UnExistingEntities_Test_Helpers
    {
        /// <summary>
        /// Creates an existing test entity.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <param name="table">The table.</param>
        /// <returns></returns>
        public static Object CreateUnExistingTestEntity(this ITable table)
        {

            var entity = Activator.CreateInstance(table.EntityType);

            if (table.EntityType == typeof(Employee))
            {
                return FillEmployee(entity as Employee);
            }
            else if (table.EntityType == typeof(Customers))
            {
                return FillCustomer(entity as Customers);
            }
            else if (table.EntityType == typeof(CustomerCustomerDemo))
            {
                return FillCustomerCustomerDemo(entity as CustomerCustomerDemo);
            }
            else if (table.EntityType == typeof(CustomerDemographics))
            {
                return FillCustomerDemographics(entity as CustomerDemographics);
            }
            else if (table.EntityType == typeof(Categories))
            {
                return FillCategory(entity as Categories);
            }
            else if (table.EntityType == typeof(EmployeeTerritories))
            {
                return FillEmployeeTerritory(entity as EmployeeTerritories);
            }
            else if (table.EntityType == typeof(OrderDetails))
            {
                return FillOrderDetails(entity as OrderDetails);
            }
            else if (table.EntityType == typeof(Orders))
            {
                return FillOrder(entity as Orders);
            }
            else if (table.EntityType == typeof(Products))
            {
                return FillProduct(entity as Products);
            }
            else if (table.EntityType == typeof(Region))
            {
                return FillRegion(entity as Region);
            }
            else if (table.EntityType == typeof(Shippers))
            {
                return FillShipper(entity as Shippers);
            }
            else if (table.EntityType == typeof(Suppliers))
            {
                return FillSupplier(entity as Suppliers);
            }
            else if (table.EntityType == typeof(Territories))
            {
                return FillTerritory(entity as Territories);
            }


            return entity;
        }

        private static object FillCustomerDemographics(CustomerDemographics customerDemographics)
        {

            return customerDemographics;
        }

        private static object FillCustomerCustomerDemo(CustomerCustomerDemo customerCustomerDemo)
        {
            
            return customerCustomerDemo;
        }

        private static object FillTerritory(Territories territories)
        {
            territories.TerritoryId = "9999";
            territories.TerritoryDescription = "Westboro";
            territories.Region = new Region { Id = 1 };
            return territories;
        }

        private static object FillSupplier(Suppliers suppliers)
        {
            suppliers.SupplierId = 1;
            suppliers.CompanyName = "Exotic Liquids";
            return suppliers;
        }

        private static object FillShipper(Shippers shippers)
        {
            shippers.ShipperId = 1;
            shippers.CompanyName = "Speedy Express";
            shippers.Phone = "(503) 555-9831";
            return shippers;
        }

        private static object FillRegion(Region region)
        {
            region.Id = 5;
            region.Description = "Eastern";
            return region;
        }

        private static object FillProduct(Products products)
        {
            products.ProductId = 11;
            products.ProductName = "testing";
            return products;
        }

        private static object FillOrder(Orders orders)
        {
            orders.OrderId = 10248;
            orders.OrderDate = new DateTime(2000, 10, 3);
            orders.RequiredDate = new DateTime(2000, 10, 3);
            orders.ShippedDate = new DateTime(2000, 10, 3);
            return orders;
        }

        private static object FillOrderDetails(OrderDetails orderDetails)
        {
            orderDetails.Order = new Orders { OrderId = 10248 };
            orderDetails.Product = new Products { ProductId = 10 };
            orderDetails.Quantity = 1000;
            return orderDetails;
        }

        private static object FillEmployeeTerritory(EmployeeTerritories employeeTerritories)
        {
            employeeTerritories.Employee = new Employee { EmployeeId = 8 };
            employeeTerritories.Territories = new Territories { TerritoryId = "19713" };
            return employeeTerritories;
        }

        private static object FillCategory(Categories categories)
        {
            categories.CategoryId = 1;
            categories.CategoryName = "Condiments";
            categories.Description = "Sweet and savory sauces, relishes, spreads, and seasonings...";
            return categories;
        }

        private static object FillCustomer(Customers customers)
        {
            customers.CustomerId = "SERG";
            customers.CompanyName = "Ana Trujillo Emparedados y helados";
            customers.ContactName = "Ana Trujillo";
            customers.ContactTitle = "Owner";
            return customers;
        }

        private static Object FillEmployee(Employee employee)
        {

            employee.EmployeeId = 100;
            employee.FirstName = "test name";
            employee.LastName = "test last name";
            employee.BirthDate = new DateTime(1983, 4, 16);
            employee.HireDate = new DateTime(1999, 4, 16);
            employee.Photo = new Byte[10];

            return employee;
        }

    }
}
