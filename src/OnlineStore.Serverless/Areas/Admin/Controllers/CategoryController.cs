using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Filters;
using MSToolKit.Core.IO.Abstraction;
using MSToolKit.Core.IO.AmazonFileManagment.Abstraction;
using OnlineStore.Common.Notifications;
using OnlineStore.Common.ViewModels.Category;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Areas.Admin.Controllers
{
    public class CategoryController : BaseAdminController
    {
        private readonly AwsFunctionOptions awsFunctionOptions;
        private readonly ICategoryService categoryService;
        private readonly IProductService productService;
        private readonly IPictureService pictureService;
        private readonly IFileProcessor fileProcessor;
        private readonly IAmazonS3FileManager amazonS3FileManager;
        private readonly ILogger<CategoryController> logger;

        public CategoryController(
            IOptions<EnvironmentOptions> environmentOptions,
            IOptions<AwsFunctionOptions> awsFunctionOptions,
            ICategoryService categoryService,
            IProductService productService,
            IPictureService pictureService,
            IFileProcessor fileProcessor,
            IAmazonS3FileManager amazonS3FileManager,
            ILogger<CategoryController> logger) : base(environmentOptions)
        {
            this.awsFunctionOptions = awsFunctionOptions.Value;
            this.categoryService = categoryService;
            this.productService = productService;
            this.pictureService = pictureService;
            this.fileProcessor = fileProcessor;
            this.amazonS3FileManager = amazonS3FileManager;
            this.logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var models = await this.categoryService.GetAsync();

            return View(models);
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Details(string id, int pageIndex = 1)
        {
            var model = await this.categoryService
                .GetAsync(id, pageIndex, WebConstants.ProductsCountPerPage);

            if (model == null)
            {
                return this.RedirectToIndex();
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create(CategoryCreateViewModel model)
        {
            var result = await this.categoryService.AddAsync(model.Name);

            this.ShowNotification(result);

            return RedirectToIndex();
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Update(string id)
        {
            var model = await this.categoryService.GetForUpdate(id);

            if (model == null)
            {
                return RedirectToIndex();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Update(CategoryUpdateViewModel model, IFormFile picture)
        {
            /// Picture is not updated, so update just category's name:
            if (picture == null)
            {
                await this.categoryService.UpdateAsync(model);

                return RedirectToIndex();
            }

            if (!picture.IsImage())
            {
                this.ShowNotification(
                    $"{picture.FileName} is not a valid image file!", 
                    NotificationType.Warning);

                return RedirectToIndex();
            }

            if (model.PictureUrl != null)
            {
                /// Removes the old picture from S3 bucket:
                await this.amazonS3FileManager
                    .RemoveFileByUrlAsync(this.awsFunctionOptions.BucketName, model.PictureUrl);
            }

            var uniqueFileName = this.fileProcessor.GetUniqueFileName(picture.FileName);
            var stream = picture.OpenReadStream();

            /// Upload the new picture to Amazon S3 bucket:
            var pictureUrl = await this.amazonS3FileManager.UploadFileAsync(
                this.awsFunctionOptions.BucketName,
                stream,
                uniqueFileName,
                S3CannedACL.PublicRead);

            model.PictureUrl = pictureUrl;

            await this.categoryService.UpdateAsync(model);

            return RedirectToIndex();
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Delete(string id)
        {
            var category = await this.categoryService.GetForDeleteAsync(id);

            if (!string.IsNullOrWhiteSpace(category.PictureUrl))
            {
                await this.amazonS3FileManager
                    .RemoveFileByUrlAsync(this.awsFunctionOptions.BucketName, category.PictureUrl);
            }
            
            foreach (var productId in category.ProductsIds)
            {
                var pictures = await this.pictureService.GetByProductIdAsync(productId);
                foreach (var picture in pictures)
                {
                    await this.pictureService.RemoveAsync(picture.Id);
                    await this.amazonS3FileManager
                        .RemoveFileByUrlAsync(this.awsFunctionOptions.BucketName, picture.Url);
                }

                await this.productService.RemoveAsync(productId);
            }

            await this.categoryService.RemoveAsync(id);

            return RedirectToIndex();
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> RemovePicture(string categoryId)
        {
            var model = await this.categoryService.GetForUpdate(categoryId);
            if (string.IsNullOrWhiteSpace(model.PictureUrl))
            {
                this.ShowNotification(
                    "No picture for removing", NotificationType.Warning);

                return RedirectToAction(nameof(this.Update), new { id = categoryId });
            }

            /// Removes the picture from S3 bucket:
            await this.amazonS3FileManager
                .RemoveFileByUrlAsync(this.awsFunctionOptions.BucketName, model.PictureUrl);
            model.PictureUrl = default(string);

            await this.categoryService.UpdateAsync(model);

            return RedirectToAction(nameof(this.Update), new { id = categoryId });
        }

        private IActionResult RedirectToIndex()
        {
            return RedirectToAction(nameof(this.Index));
        }
    }
}