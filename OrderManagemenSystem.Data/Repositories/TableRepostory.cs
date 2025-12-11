using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using OrderManagemenSystem.Common.DTOs;
using OrderManagemenSystem.Data.Contracts;
using OrderManagemenSystem.Data.Entities;
using OrderManagemenSystem.Data.Entities.Table;

namespace OrderManagemenSystem.Data.Repositories
{
    public class CategoryRepostory : BaseRepository<Category>, ICategoryRepository
    {
        public CategoryRepostory(OMSContext oMSContext) : base(oMSContext)
        {
        }
    }

    public class ItemRepostory : BaseRepository<Item>, IItemRepository
    {
        public ItemRepostory(OMSContext oMSContext) : base(oMSContext)
        {
        }
    }

    public class OrderRepostory : BaseRepository<Order>, IOrderRepository
    {
        private readonly OMSContext _context;
        public OrderRepostory(OMSContext oMSContext) : base(oMSContext)
        {
            _context = oMSContext;
        }

        public async Task<IList<SpOrderDetail>> GetOrderDetails(GridSearchRequest gridSearchRequest)
        {
            var parameters = new List<SqlParameter>();
            parameters.Add(new SqlParameter("@SearchText", string.IsNullOrEmpty(gridSearchRequest.SearchText) ? DBNull.Value : gridSearchRequest.SearchText));
            parameters.Add(new SqlParameter("@SortCoulmn", gridSearchRequest.SortColumn));
            parameters.Add(new SqlParameter("@SortType", gridSearchRequest.SortType));
            parameters.Add(new SqlParameter("@PageNumber", gridSearchRequest.PageNumber));
            parameters.Add(new SqlParameter("@PageSize", gridSearchRequest.PageSize));
            return await _context.SpOrderDetails.FromSqlRaw("EXEC SP_OrderDetails @SearchText,@SortCoulmn,@SortType,@PageNumber,@PageSize", parameters.ToArray()).ToListAsync();
        }
    }

    public class VwOrderDetailRepostory : BaseRepository<VwOrderDetail>, IVwOrderDetailRepository
    {
        public VwOrderDetailRepostory(OMSContext oMSContext) : base(oMSContext)
        {
        }
    }
}
