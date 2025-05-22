using Microsoft.EntityFrameworkCore;
using SQRBackend.Data;
using SQRBackend.Models;
using SQRBackend.Models.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SQRBackend.Services
{
    public class OrderService : IOrderService
    {
        private readonly SQRDbContext _context;

        public OrderService(SQRDbContext context)
        {
            _context = context;
        }

        public async Task<List<OrderDto>> GetOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.Product)
                .Select(o => new OrderDto
                {
                    Order = o.OrderId,
                    Quantity = o.Quantity,
                    ProductCode = o.ProductCode,
                    ProductDescription = o.Product != null ? o.Product.ProductDescription : null,
                    Image = o.Product != null ? o.Product.Image : null,
                    CycleTime = o.Product != null ? o.Product.CycleTime : 0,
                    Materials = _context.ProductMaterials
                        .Where(pm => pm.ProductCode == o.ProductCode)
                        .Include(pm => pm.Material)
                        .Select(pm => new MaterialDto
                        {
                            MaterialCode = pm.MaterialCode,
                            MaterialDescription = pm.Material != null ? pm.Material.MaterialDescription : null
                        }).ToList()
                }).ToListAsync();
        }

        public async Task<List<ProductionDto>> GetProductionAsync(string email)
        {
            if (!await _context.Users.AnyAsync(u => u.Email == email))
                return new List<ProductionDto>();

            return await _context.Productions
                .Where(p => p.Email == email)
                .Select(p => new ProductionDto
                {
                    Id = p.Id,
                    Order = p.OrderId,
                    Date = p.Date.ToString("M/dd/yyyy h:mm:ss tt"),
                    Quantity = p.Quantity,
                    MaterialCode = p.MaterialCode,
                    CycleTime = p.CycleTime
                }).ToListAsync();
        }

        public async Task<SetProductionResponseDto> SetProductionAsync(SetProductionDto dto)
        {
            // Validate Email
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (user == null)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Usuário não cadastrado!" };

            // Validate Order
            var order = await _context.Orders.FirstOrDefaultAsync(o => o.OrderId == dto.Order);
            if (order == null)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Ordem não cadastrada!" };

            // Validate Date
            if (!DateTime.TryParse($"{dto.ProductionDate} {dto.ProductionTime}", out var productionDateTime))
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Data inválida!" };

            if (productionDateTime < user.InitialDate || productionDateTime > user.EndDate)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Data fora do período válido para o usuário!" };

            // Validate Quantity
            if (dto.Quantity <= 0 || dto.Quantity > order.Quantity)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Quantidade inválida!" };

            // Validate Material
            var materialExists = await _context.ProductMaterials
                .AnyAsync(pm => pm.ProductCode == order.ProductCode && pm.MaterialCode == dto.MaterialCode);
            if (!materialExists)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Material não associado à ordem!" };

            // Validate CycleTime
            var product = await _context.Products.FirstOrDefaultAsync(p => p.ProductCode == order.ProductCode);
            if (product == null)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Produto não encontrado!" };

            string description = "Apontamento registrado com sucesso!";
            if (dto.CycleTime <= 0)
                return new SetProductionResponseDto { Status = 201, Type = "E", Description = "Falha no apontamento - Tempo de ciclo inválido!" };
            if (dto.CycleTime < product.CycleTime)
                description = "Apontamento registrado, mas o tempo de ciclo é menor que o cadastrado!";

            // Save Production
            var production = new Production
            {
                Email = dto.Email,
                OrderId = dto.Order,
                Date = productionDateTime,
                Quantity = dto.Quantity,
                MaterialCode = dto.MaterialCode,
                CycleTime = dto.CycleTime
            };
            _context.Productions.Add(production);
            await _context.SaveChangesAsync();

            return new SetProductionResponseDto { Status = 200, Type = "S", Description = description };
        }
    }
}