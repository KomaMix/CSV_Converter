using CSV_Converter.Data;
using CSV_Converter.Exceptions;
using CSV_Converter.Models;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
using System.Formats.Asn1;
using System.Globalization;

namespace CSV_Converter.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderByIdAsync(int id)
        {
            var order = await _context.Orders.FindAsync(id);

            if (order == null)
            {
                throw new NotFoundException($"Заказ с {id} не найден");
            }

            return order;
        }

        public async Task<List<Order>> GetOrdersAsync()
        {
            return await _context.Orders.ToListAsync();
        }

        public async Task LoadData(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл пуст или отсутствует");

            using (var reader = new StreamReader(file.OpenReadStream()))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                try
                {


                    var orders = csv.GetRecords<Order>().ToList();

                    _context.Orders.AddRange(orders);

                    await _context.SaveChangesAsync();
                }
                catch (Exception ex)
                {
                    throw new Exception("Произошла ошибка при обработке CSV файла", ex);
                }
            }
        }
    }
}
