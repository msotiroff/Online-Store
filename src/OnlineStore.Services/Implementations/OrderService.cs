using Microsoft.AspNetCore.Mvc.Rendering;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using MSToolKit.Core.Validation;
using OnlineStore.Common.ViewModels.Enums;
using OnlineStore.Common.ViewModels.Order;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Models.Enums;
using OnlineStore.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Services.Implementations
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository orderRepository;
        private readonly IProductRepository productRepository;
        private readonly IOrderFactory orderFactory;
        private readonly ICustomMapper customMapper;

        public OrderService(
            IOrderRepository orderRepository, 
            IProductRepository productRepository,
            IOrderFactory orderFactory,
            ICustomMapper customMapper)
        {
            this.orderRepository = orderRepository;
            this.productRepository = productRepository;
            this.orderFactory = orderFactory;
            this.customMapper = customMapper;
        }

        public async Task CreateAsync(OrderCreateViewModel model)
        {
            var orderItems = new List<OrderItem>();

            foreach (var item in model.ShoppingCart.Items)
            {
                orderItems.Add(new OrderItem
                {
                    Count = item.Count,
                    Product = await this.productRepository.GetAsync(item.Product.Id)
                });
            }

            var order = this.orderFactory.CreateInstance(
                model.BrowserId,
                model.CustomerName,
                model.CustomerPhoneNumber,
                model.DeliveryAddress,
                model.UserId,
                model.ShoppingCart.TotalAmount,
                orderItems);

            await this.orderRepository.AddAsync(order);

            foreach (var item in order.Items)
            {
                var product = await this.productRepository.GetAsync(item.Product.Id);

                product.Availability -= item.Count;

                await this.productRepository.UpdateAsync(product);
            }
        }

        public async Task<IEnumerable<OrderAdminIndexViewModel>> GetAsync(
            int pageIndex, int itemsPerPage, string orderMember, SortDirection sortDirection)
        {
            var dbOrders = (await this.orderRepository.GetAsync())
                .Select(o => this.customMapper.Map<Order, OrderAdminIndexViewModel>(o));
            var orders = sortDirection == SortDirection.Ascending
                ? dbOrders.OrderByMember(orderMember).ToPaginatedList(pageIndex, itemsPerPage)
                : dbOrders.OrderByMemberDescending(orderMember).ToPaginatedList(pageIndex, itemsPerPage);

            orders.ForEach(o =>
            {
                o.AllStates = Enum
                .GetNames(typeof(OrderState))
                .Select(os => new SelectListItem(os, os, os.Equals(o.State.ToString())));
            });

            return orders;
        }

        public async Task<OrderDetailsViewModel> GetAsync(string id)
        {
            var order = await this.orderRepository.GetAsync(id);

            CoreValidator.ThrowIfNull(order);

            var model = this.customMapper.Map<Order, OrderDetailsViewModel>(order);
            model.Products = order.Items
                .ToDictionary(
                    oi => this.customMapper.Map<Product, ProductSimpleViewModel>(oi.Product), 
                    oi => oi.Count);

            return model;
        }

        public async Task<OrderAdminDetailsViewModel> GetForAdminAsync(string id)
        {
            var order = await this.orderRepository.GetAsync(id);

            CoreValidator.ThrowIfNull(order);

            var model = this.customMapper.Map<Order, OrderAdminDetailsViewModel>(order);
            model.Products = order.Items.ToDictionary(k => k.Product.Name, v => v.Count);
            
            return model;
        }

        public async Task<IEnumerable<OrderIndexViewModel>> GetOrdersByUserIdAsync(
            string userId, int pageIndex, int itemsPerPage)
        {
            var orders = (await this.orderRepository.FilterAsync(nameof(Order.UserId), userId))
                .Select(o => this.customMapper.Map<Order, OrderIndexViewModel>(o))
                .AsQueryable()
                .ToPaginatedList(pageIndex, itemsPerPage);
            
            return orders;
        }

        public async Task UpodateStateAsync(string orderId, string newState)
        {
            var order = await this.orderRepository.GetAsync(orderId);

            CoreValidator.ThrowIfNull(order);
            order.State = Enum.Parse<OrderState>(newState);

            await this.orderRepository.UpdateAsync(order);
        }
    }
}
