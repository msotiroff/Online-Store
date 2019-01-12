using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using MSToolKit.Core.Validation;
using OnlineStore.Common.ViewModels.OrderItem;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.Common.ViewModels.ShoppingCart;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ShoppingCartService : IShoppingCartService
    {
        private readonly ILogger<IShoppingCartService> logger;
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;
        private readonly IPictureRepository pictureRepository;
        private readonly IShoppingCartFactory shoppingCartFactory;
        private readonly ICustomMapper customMapper;

        public ShoppingCartService(
            ILogger<IShoppingCartService> logger,
            IShoppingCartRepository shoppingCartRepository,
            IProductRepository productRepository,
            IPictureRepository pictureRepository,
            IShoppingCartFactory shoppingCartFactory,
            ICustomMapper customMapper)
        {
            this.logger = logger;
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
            this.pictureRepository = pictureRepository;
            this.shoppingCartFactory = shoppingCartFactory;
            this.customMapper = customMapper;
        }

        public async Task AddProductAsync(string browserId, string productId, int count)
        {
            CoreValidator.ThrowIfAnyNullOrWhitespace(browserId, productId);

            var shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);
            
            if (shoppingCart == null)
            {
                var newCart = this.shoppingCartFactory.CreateInstance(browserId);
                await this.shoppingCartRepository.AddAsync(newCart);
                shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);
            }

            var product = await this.productRepository.GetAsync(productId);

            CoreValidator.ThrowIfNull(product);

            if (product.IsDeleted)
            {
                throw new InvalidOperationException($"Product with id: {productId} is deleted!");
            }

            var item = shoppingCart.Items.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null)
            {
                item = new OrderItem
                {
                    Product = product,
                    Count = 0
                };
            }

            item.Count += count;
            if (item.Count > item.Product.Availability)
            {
                return;
            }

            var newItems = shoppingCart.Items.Where(i => i.Product.Id != productId).ToList();
            newItems.Add(item);

            shoppingCart.UpdateItems(newItems);

            await this.shoppingCartRepository.UpdateAsync(shoppingCart);
        }

        public async Task RemoveAsync(string browserId)
        {
            var shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);

            await this.shoppingCartRepository.RemoveAsync(shoppingCart);
        }

        public async Task<ShoppingCartIndexViewModel> GetAsync(string browserId)
        {
            CoreValidator.ThrowIfNullOrWhitespace(browserId);

            var shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);
            
            if (shoppingCart == null)
            {
                var newCart = this.shoppingCartFactory.CreateInstance(browserId);
                await this.shoppingCartRepository.AddAsync(newCart);
                shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);
            }

            var model = this.customMapper.Map<ShoppingCart, ShoppingCartIndexViewModel>(shoppingCart);
            var modelItems = new List<OrderItemIndexViewModel>();

            foreach (var item in shoppingCart.Items)
            {
                modelItems.Add(new OrderItemIndexViewModel
                {
                    Count = item.Count,
                    Product = new ProductShoppingCartViewModel
                    {
                        Id = item.Product.Id,
                        Name = item.Product.Name,
                        Price = item.Product.Price,
                        Availability = item.Product.Availability,
                        MainPictureUrl = (await this.pictureRepository.GetByProductId(item.Product.Id))
                        .FirstOrDefault()
                        ?.Url,
                        ShortDescription = item.Product.Description.Length > 50
                        ? item.Product.Description.Substring(0, 50) + "..."
                        : item.Product.Description
                    }
                });
            }

            model.Items = modelItems.OrderBy(i => i.Product.Name).ToList();
            
            return model;
        }

        public async Task RemoveProductAsync(string browserId, string productId)
        {
            CoreValidator.ThrowIfAnyNullOrWhitespace(browserId, productId);

            var shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);

            CoreValidator.ThrowIfNull(shoppingCart);

            var orderItems = shoppingCart.Items;
            var item = orderItems.FirstOrDefault(i => i.Product.Id == productId);
            if (item == null)
            {
                return;
            }

            var newItems = orderItems.Where(oi => oi.Product.Id != productId).ToList();

            shoppingCart.UpdateItems(newItems);

            await this.shoppingCartRepository.UpdateAsync(shoppingCart);
        }

        public async Task SubtractProductAsync(string browserId, string productId, int count)
        {
            CoreValidator.ThrowIfAnyNullOrWhitespace(browserId, productId);

            var shoppingCart = await this.shoppingCartRepository.GetAsync(browserId);

            CoreValidator.ThrowIfNull(shoppingCart);

            var orderItems = shoppingCart.Items;
            var item = orderItems.FirstOrDefault(i => i.Product.Id == productId && i.Count > 0);
            if (item == null)
            {
                return;
            }

            item.Count -= count;
            if (item.Count <= 0)
            {
                orderItems = orderItems.Where(oi => oi.Product.Id != productId).ToList();
            }

            shoppingCart.UpdateItems(orderItems);

            await this.shoppingCartRepository.UpdateAsync(shoppingCart);
        }
    }
}
