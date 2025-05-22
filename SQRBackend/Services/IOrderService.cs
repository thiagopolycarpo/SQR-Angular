using SQRBackend.Models.Dtos;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SQRBackend.Services
{
    public interface IOrderService
    {
        Task<List<OrderDto>> GetOrdersAsync();
        Task<List<ProductionDto>> GetProductionAsync(string email);
        Task<SetProductionResponseDto> SetProductionAsync(SetProductionDto dto);
    }
}