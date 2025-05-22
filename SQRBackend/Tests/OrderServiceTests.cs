using Microsoft.EntityFrameworkCore;
   using Moq;
   using SQRBackend.Data;
   using SQRBackend.Models;
   using SQRBackend.Models.Dtos;
   using SQRBackend.Services;
   using System;
   using System.Collections.Generic;
   using System.Linq;
   using System.Threading.Tasks;
   using Xunit;

   namespace SQRBackend.Tests
   {
       public class OrderServiceTests
       {
           private readonly SQRDbContext _context;
           private readonly OrderService _service;

           public OrderServiceTests()
           {
               var options = new DbContextOptionsBuilder<SQRDbContext>()
                   .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                   .Options;
               _context = new SQRDbContext(options);
               _service = new OrderService(_context);

               // Seed data
               _context.Users.Add(new User
               {
                   Email = "teste@sqr.com.br",
                   Name = "Test User",
                   InitialDate = DateTime.Parse("2021-01-01"),
                   EndDate = DateTime.Parse("2025-12-31")
               });
               _context.Orders.Add(new Order
               {
                   OrderId = "111",
                   Quantity = 100,
                   ProductCode = "abc",
                   Product = new Product // Add related Product
                   {
                       ProductCode = "abc",
                       ProductDescription = "xxx",
                       Image = "0x000001",
                       CycleTime = 30.3m
                   }
               });
               _context.Products.Add(new Product
               {
                   ProductCode = "abc",
                   ProductDescription = "xxx",
                   Image = "0x000001",
                   CycleTime = 30.3m
               });
               _context.Materials.Add(new Material
               {
                   MaterialCode = "aaa",
                   MaterialDescription = "desc1"
               });
               _context.ProductMaterials.Add(new ProductMaterial
               {
                   ProductCode = "abc",
                   MaterialCode = "aaa"
               });
               _context.SaveChanges();
           }

           [Fact]
           public async Task GetOrdersAsync_ReturnsOrders()
           {
               var result = await _service.GetOrdersAsync();
               Assert.Single(result);
               Assert.Equal("111", result[0].Order);
           }

           [Fact]
           public async Task GetProductionAsync_ValidEmail_ReturnsProductions()
           {
               var result = await _service.GetProductionAsync("teste@sqr.com.br");
               Assert.Empty(result);
           }

           [Fact]
           public async Task SetProductionAsync_ValidInput_ReturnsSuccess()
           {
               var dto = new SetProductionDto
               {
                   Email = "teste@sqr.com.br",
                   Order = "111",
                   ProductionDate = "2021-02-01",
                   ProductionTime = "10:30:00",
                   Quantity = 50,
                   MaterialCode = "aaa",
                   CycleTime = 30.3m
               };
               var result = await _service.SetProductionAsync(dto);
               Assert.Equal(200, result.Status);
               Assert.Equal("S", result.Type);
           }

           [Fact]
           public async Task SetProductionAsync_InvalidEmail_ReturnsError()
           {
               var dto = new SetProductionDto
               {
                   Email = "invalid@sqr.com.br",
                   Order = "111",
                   ProductionDate = "2021-02-01",
                   ProductionTime = "10:30:00",
                   Quantity = 50,
                   MaterialCode = "aaa",
                   CycleTime = 30.3m
               };
               var result = await _service.SetProductionAsync(dto);
               Assert.Equal(201, result.Status);
               Assert.Equal("E", result.Type);
               Assert.Contains("Usuário não cadastrado", result.Description);
           }
       }
   }