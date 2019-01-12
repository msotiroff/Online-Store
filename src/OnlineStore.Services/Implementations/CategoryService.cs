using MSToolKit.Core.Extensions;
using MSToolKit.Core.Mapping.CustomMapping.Abstraction;
using MSToolKit.Core.Validation;
using OnlineStore.Common.ViewModels.Category;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.DAL.Interfaces;
using OnlineStore.Factories.Interfaces;
using OnlineStore.Models;
using OnlineStore.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OnlineStore.Services.Implementations
{
    public class CategoryService : ICategoryService
    {
        private readonly ICategoryFactory factory;
        private readonly ICategoryRepository categoryRepository;
        private readonly IProductRepository productRepository;
        private readonly IPictureRepository pictureRepository;
        private readonly IProductService productService;
        private readonly ICustomMapper customMapper;

        public CategoryService(
            ICategoryFactory factory,
            ICategoryRepository repository,
            IProductRepository productRepository,
            IPictureRepository pictureRepository,
            IProductService productService,
            ICustomMapper customMapper)
        {
            this.factory = factory;
            this.categoryRepository = repository;
            this.productRepository = productRepository;
            this.pictureRepository = pictureRepository;
            this.productService = productService;
            this.customMapper = customMapper;
        }

        public async Task<ActionExecutionResult> AddAsync(string name)
        {
            var category = this.factory.CreateInstance(name);
            var validation = category.GetValidationResult();

            if (!validation.Success)
            {
                return new ActionExecutionResult(validation.Errors);
            }

            await this.categoryRepository.AddAsync(category);

            return new ActionExecutionResult();
        }

        public async Task<IEnumerable<CategoryIndexViewModel>> GetAsync()
        {
            return (await this.categoryRepository.GetAsync())
                .OrderBy(c => c.DateCreated)
                .Select(c => this.customMapper.Map<Category, CategoryIndexViewModel>(c))
                .ToList();
        }

        public async Task<CategoryDetailsViewModel> GetAsync(string id, int pageIndex, int productsPerPage)
        {
            CoreValidator.ThrowIfNullOrWhitespace(id);

            var category = await this.categoryRepository.GetAsync(id);

            if (category == default(Category))
            {
                return null;
            }

            var products = await this.productRepository.GetByCategoryId(id);

            var productModels = products
                .Where(p => !p.IsDeleted)
                .Select(pr => this.customMapper.Map<Product, ProductConciseViewModel>(pr))
                .AsQueryable()
                .ToPaginatedList(pageIndex, productsPerPage);

            foreach (var productModel in productModels)
            {
                productModel.CategoryName = category.Name;
                productModel.MainPictureUrl = 
                    (await this.pictureRepository.GetByProductId(productModel.Id))
                    .FirstOrDefault()
                    ?.Url;
            }

            var model = this.customMapper.Map<Category, CategoryDetailsViewModel>(category);
            model.Products = productModels;

            return model;
        }

        public async Task<CategoryDeleteViewModel> GetForDeleteAsync(string id)
        {
            var category = await this.categoryRepository.GetAsync(id);

            CoreValidator.ThrowIfNull(category);

            var model = this.customMapper.Map<Category, CategoryDeleteViewModel>(category);
            model.ProductsIds = (await this.productService.GetByCategoryIdAsync(id)).Select(p => p.Id);

            return model;
        }

        public async Task<CategoryUpdateViewModel> GetForUpdate(string id)
        {
            var category = await this.categoryRepository.GetAsync(id);

            if (category == default(Category))
            {
                return null;
            }

            var model = this.customMapper.Map<Category, CategoryUpdateViewModel>(category);

            return model;
        }

        public async Task RemoveAsync(string id)
        {
            var category = await this.categoryRepository.GetAsync(id);

            CoreValidator.ThrowIfNull(category);

            await this.categoryRepository.RemoveAsync(category);
        }

        public async Task UpdateAsync(CategoryUpdateViewModel model)
        {
            var category = this.customMapper.Map<CategoryUpdateViewModel, Category>(model);
            
            await this.categoryRepository.UpdateAsync(category);
        }
    }
}
