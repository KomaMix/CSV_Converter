using CSV_Converter.Data;
using CSV_Converter.Exceptions;
using CSV_Converter.Mappers;
using CSV_Converter.Models;
using CsvHelper;
using Microsoft.EntityFrameworkCore;
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
            return await _context.Orders.Take(100).ToListAsync();
        }

        public async Task LoadData(string fileName)
        {
            List<Order> orders = new List<Order>();

			var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "files", fileName);

			using (var reader = new StreamReader(path))
            using (var csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Context.RegisterClassMap<OrderMap>();
                csvReader.Read();
                csvReader.ReadHeader();

                while (csvReader.Read())
                {
                    var order = csvReader.GetRecord<Order>();

					//order.OrderDate = DateTime.SpecifyKind(order.OrderDate, DateTimeKind.Utc);
					//order.ShipDate = DateTime.SpecifyKind(order.ShipDate, DateTimeKind.Utc);

					orders.Add(order);
                }
            }

            await _context.Orders.AddRangeAsync(orders);
            await _context.SaveChangesAsync();

            
        }
    }
}
