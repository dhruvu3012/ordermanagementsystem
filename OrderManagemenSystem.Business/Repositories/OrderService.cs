using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagemenSystem.Business.Contracts;
using OrderManagemenSystem.Common.DTOs;
using OrderManagemenSystem.Data.Contracts;
using OrderManagemenSystem.Data.Entities;
using OrderManagemenSystem.Data.Entities.Table;

namespace OrderManagemenSystem.Business.Repositories
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly OMSContext _context;
        
        public OrderService(IOrderRepository orderRepository, OMSContext context) {
            _orderRepository = orderRepository;
            _context = context;
        }

        public async Task<int> DeleteOrder(int id)
        {
            var getOrder = await _context.Orders.FirstOrDefaultAsync(o => o.Id == id);
            if(getOrder!=null)
            {
                _context.Orders.Remove(getOrder);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<GridSearchResult> GetOrderDetails(GridSearchRequest gridSearchRequest)
        {
            var result = await _orderRepository.GetOrderDetails(gridSearchRequest);

            return new GridSearchResult(gridSearchRequest, result, result.Select(x => x.TotalCount).FirstOrDefault());
        }

        public async Task<int> SaveOrder(OrderRequest order)
        {
            if(order.Orders.Any())
            {
                foreach(var o in order.Orders)
                {
                    await _context.Orders.AddAsync(new Order()
                    {
                        CustomerName = order.CustomerName,
                        Date = order.OrderDate,
                        ItemId = o.ItemID,
                        Qty = o.Qty,
                    });
                }
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<int> UpdateOrder(OrderUpdateRequest order)
        {
            var getOrder = await _orderRepository.GetAsync(order.Id);
            if (getOrder != null)
            {
                getOrder.Qty = order.Qty;
                getOrder.Date = order.OrderDate;
                getOrder.CustomerName = order.CustomerName;
                getOrder.ItemId = order.ItemID;

                await _orderRepository.UpdateAsync(getOrder);
                return await _context.SaveChangesAsync();
            }
            return 0;
        }

        public async Task<IEnumerable<Category>> GetAllCategories()
        {
            return await _context.Categories.Include(x => x.Items).ToListAsync();            
        }

        public async Task<VwOrderDetail> GetVwOrderDetail(int id) => await _context.VwOrderDetails.FirstOrDefaultAsync(x => x.Id == id);
    }
}
