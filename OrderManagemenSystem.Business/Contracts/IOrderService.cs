using OrderManagemenSystem.Common.DTOs;
using OrderManagemenSystem.Data.Entities.Table;

namespace OrderManagemenSystem.Business.Contracts
{
    public interface IOrderService
    {
        Task<GridSearchResult> GetOrderDetails(GridSearchRequest gridSearchRequest);
        Task<int> SaveOrder(OrderRequest order);
        Task<int> DeleteOrder(int id);
        Task<int> UpdateOrder(OrderUpdateRequest order);
        Task<IEnumerable<Category>> GetAllCategories();
        Task<VwOrderDetail> GetVwOrderDetail(int id);
    }
}
