using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using MSToolKit.Core.Validation;
using OnlineStore.Common.ViewModels.Picture;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;

namespace OnlineStore.Services.Implementations
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IPictureRepository pictureRepository;
        private readonly IProductFactory productFactory;
        private readonly ICustomMapper customMapper;

        public ProductService(
            IProductRepository productRepository,
            ICategoryRepository categoryRepository,
            IPictureRepository pictureRepository,
            IProductFactory productFactory,
            ICustomMapper customMapper)
        {
            this.productRepository = productRepository;
            this.categoryRepository = categoryRepository;
            this.pictureRepository = pictureRepository;
            this.productFactory = productFactory;
            this.customMapper = customMapper;
        }

        public async Task<string> CreateAsync(ProductCreateViewModel model)
        {
            CoreValidator.ThrowIfNull(model);

            var product = this.productFactory.CreateInstance(
                model.Name, model.Description, model.Price, model.Availability, model.CategoryId);

            await this.productRepository.AddAsync(product);

            return product.Id;
        }

        public async Task<IEnumerable<ProductConciseViewModel>> FilterAsync(
            string searchTerm, int pageIndex, int itemsPerPage)
        {
            if (searchTerm == null)
            {
                searchTerm = string.Empty;
            }

            var products = (await this.productRepository.GetAsync())
                .Where(p => p.Name.ToLower().Contains(searchTerm.ToLower()) 
                    || p.Description.ToLower().Contains(searchTerm.ToLower()))
                .Select(p => this.customMapper.Map<Product, ProductConciseViewModel>(p))
                .ToPaginatedList(pageIndex, itemsPerPage);

            foreach (var product in products)
            {
                product.MainPictureUrl =
                    (await this.pictureRepository.GetByProductId(product.Id))
                    .FirstOrDefault()
                    ?.Url;
            }

            return products;
        }

        public async Task<ProductDetailsViewModel> GetAsync(string id)
        {
            var product = await this.productRepository.GetAsync(id);
            CoreValidator.ThrowIfNull(product);
            this.EnsureNotDeleted(product);

            var category = await this.categoryRepository.GetAsync(product.CategoryId);
            var pictures = await this.pictureRepository
                .FilterAsync(nameof(Picture.EntityId), product.Id);

            CoreValidator.ThrowIfAnyNull(category, pictures);

            var model = this.customMapper.Map<Product, ProductDetailsViewModel>(product);
            model.CategoryName = category.Name;
            model.Pictures = pictures
                .Select(pic => this.customMapper.Map<Picture, PictureConciseViewModel>(pic));

            return model;
        }

        public Task<IEnumerable<Product>> GetByCategoryIdAsync(string categoryId)
        {
            var conditions = new Dictionary<string, object>
            {
                [nameof(Product.CategoryId)] = categoryId,
                [nameof(Product.IsDeleted)] = false
            };

            return this.productRepository.FilterAsync(conditions);
        }

        public async Task<ProductUpdateViewModel> GetForUpdateAsync(string id)
        {
            var product = await this.productRepository.GetAsync(id);
            CoreValidator.ThrowIfNull(product);
            this.EnsureNotDeleted(product);

            var pictures = await this.pictureRepository.FilterAsync(nameof(Picture.EntityId), product.Id);
            var model = this.customMapper.Map<Product, ProductUpdateViewModel>(product);
            model.Pictures = pictures
                .Select(pic => this.customMapper.Map<Picture, PictureConciseViewModel>(pic));

            return model;
        }

        public async Task RemoveAsync(string id)
        {
            var product = await this.productRepository.GetAsync(id);

            CoreValidator.ThrowIfNull(product);

            product.IsDeleted = true;

            await this.productRepository.UpdateAsync(product);
        }

        public async Task UpdateAsync(ProductUpdateViewModel model)
        {
            CoreValidator.ThrowIfNull(model);

            var product = this.customMapper.Map<ProductUpdateViewModel, Product>(model);

            await this.productRepository.UpdateAsync(product);
        }
        
        private void EnsureNotDeleted(Product product)
        {
            if (product.IsDeleted)
            {
                throw new InvalidOperationException($"Product with id: {product.Id} is deleted!");
            }
        }
    }
}
