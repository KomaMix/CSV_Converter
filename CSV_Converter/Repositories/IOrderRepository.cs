using CSV_Converter.Data;
using CSV_Converter.Models;

namespace CSV_Converter.Repositories
{
    public interface IOrderRepository
    {
        Task LoadData(IFormFile file);
        Task<List<Order>> GetOrdersAsync();
        Task<Order> GetOrderByIdAsync(int id);
    }
}
