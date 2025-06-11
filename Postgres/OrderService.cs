using Postgres.Models;

namespace Postgres
{
    public class OrderService
    {
        // контекст бд
        private AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        /*public async Task<Order> CreateOrderAsync(Order order, OrderFile file)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                var orderFile = new OrderFile()
                {
                    OrderId = order.Id,
                    FileName = file.FileName,
                    UploadedAt = DateTime.UtcNow
                };

                _context.OrderFiles.Add(orderFile);
                await _context.SaveChangesAsync();

                await transaction.CommitAsync();
                return order;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }*/

        // метод создания заказа в бд (транзакция)
        public async Task<Order> CreateOrderAsync(Order order)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                /*var orderFile = new OrderFile()
                {
                    OrderId = order.Id,
                    FileName = file.FileName,
                    UploadedAt = DateTime.UtcNow
                };

                _context.OrderFiles.Add(orderFile);
                await _context.SaveChangesAsync();*/

                await transaction.CommitAsync();
                return order;
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }


    }
}
