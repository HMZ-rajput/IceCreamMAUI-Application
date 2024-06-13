using IceCreamMAUI.Api.Data;
using IceCreamMAUI.Api.Data.Entities;
using IceCreamMAUI.Shared.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IceCreamMAUI.Api.Services
{
    public class OrderService(DataContext context)
    {
        private readonly DataContext _context = context;

        public async Task<ResultDto> PlaceOrderAsync(OrderPlaceDto dto, Guid customerId)
        {
            var customer = await _context.Users.FirstOrDefaultAsync(u=>u.Id==customerId);
            if (customer is null)
                return ResultDto.Failure("Customer does nor exist");

            var orderItems = dto.Items.Select(i =>
            new OrderItem
            {
                IcecreamId = i.IcecreamId,
                Name = i.Name,
                Price = i.Price,
                Quantity = i.Quantity,
                Topping = i.Topping,
                Flavor = i.Flavor,
                TotalPrice = i.TotalPrice,
            });

            var order = new Order
            {
                CustomerId = customerId,
                CustomerAddress = customer.Address,
                CustomerEmail = customer.Email,
                CustomerName = customer.Name,
                OrderAt = DateTime.Now,
                TotalPrice = orderItems.Sum(o => o.TotalPrice),
                Items = orderItems.ToArray()
            };

            try
            {
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();
                return ResultDto.Success();
            }catch (Exception ex)
            {
                return ResultDto.Failure(ex.Message);
            }
        }

        public async Task<OrderDto[]> GetUserOrdersAsync(Guid userId)=>
            await _context.Orders
                .Where(o => o.CustomerId == userId)
                .Select(o => new OrderDto(o.Id, o.OrderAt, o.TotalPrice, o.Items.Count))
                .ToArrayAsync();

        public async Task<OrderItemDto[]> GetUserOrderItemsAsync(long orderId, Guid userId)=>
            await _context.OrderItems
            .Where(i=>i.OrderId==orderId && i.Order.CustomerId== userId)
            .Select(i=>new OrderItemDto(i.Id,i.IcecreamId,i.Name,i.Quantity,i.Price,i.Flavor,i.Topping))
            .ToArrayAsync();
    }
}
