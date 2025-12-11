using OrderManagemenSystem.Common.DTOs;
using OrderManagemenSystem.Data.Entities.Table;

namespace OrderManagemenSystem.Data.Contracts
{
    public interface ICategoryRepository : IBaseRepository<Category>
    {
    }

    public interface IItemRepository : IBaseRepository<Item>
    {
    }

    public interface IOrderRepository : IBaseRepository<Order>
    {
        Task<IList<SpOrderDetail>> GetOrderDetails(GridSearchRequest request);
    }

    public interface IVwOrderDetailRepository : IBaseRepository<VwOrderDetail>
    {
    }
}
