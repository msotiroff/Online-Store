using OnlineStore.Common.ViewModels.Enums;
using OnlineStore.Common.ViewModels.Order;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Services.Interfaces
{
    public interface IOrderService
    {
        Task<OrderAdminDetailsViewModel> GetForAdminAsync(string id);

        Task<OrderDetailsViewModel> GetAsync(string id);

        Task<IEnumerable<OrderAdminIndexViewModel>> GetAsync(
            int pageIndex, int itemsPerPage, string orderMember, SortDirection sortDirection);

        Task UpodateStateAsync(string orderId, string newState);

        Task CreateAsync(OrderCreateViewModel model);

        Task<IEnumerable<OrderIndexViewModel>> GetOrdersByUserIdAsync(
            string userId, int pageIndex, int itemsPerPage);
    }
}
