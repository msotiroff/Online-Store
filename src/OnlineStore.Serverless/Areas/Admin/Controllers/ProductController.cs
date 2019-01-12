using Amazon.S3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using MSToolKit.Core.Extensions;
using MSToolKit.Core.Filters;
using MSToolKit.Core.IO.Abstraction;
using MSToolKit.Core.IO.AmazonFileManagment.Abstraction;
using OnlineStore.Common.ViewModels.Product;
using OnlineStore.Models;
using OnlineStore.Serverless.Infrastructure.Options;
using OnlineStore.Services.Interfaces;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OnlineStore.Serverless.Areas.Admin.Controllers
{
    public class ProductController : BaseAdminController
    {
        private readonly AwsFunctionOptions awsFunctionOptions;
        private readonly IProductService productService;
        private readonly IPictureService pictureService;
        private readonly IFileProcessor fileProcessor;
        private readonly IAmazonS3FileManager amazonS3FileManager;

        public ProductController(
            IOptions<EnvironmentOptions> environmentOptions,
            IOptions<AwsFunctionOptions> awsFunctionOptions,
            IProductService productService,
            IPictureService pictureService,
            IFileProcessor fileProcessor,
            IAmazonS3FileManager amazonS3FileManager) : base(environmentOptions)
        {
            this.awsFunctionOptions = awsFunctionOptions.Value;
            this.productService = productService;
            this.pictureService = pictureService;
            this.fileProcessor = fileProcessor;
            this.amazonS3FileManager = amazonS3FileManager;
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Details(string id)
        {
            var model = await this.productService.GetAsync(id);

            return View(model);
        }

        [HttpGet]
        [ValidateQueryParameters]
        public IActionResult Create(string categoryId)
        {
            return View(new ProductCreateViewModel(categoryId));
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Create(ProductCreateViewModel model)
        {
            var id = await this.productService.CreateAsync(model);

            return this.RedirectToDetails(id);
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Update(string id)
        {
            var model = await this.productService.GetForUpdateAsync(id);

            return View(model);
        }

        [HttpPost]
        [ValidateModelState]
        public async Task<IActionResult> Update(
            ProductUpdateViewModel model, IEnumerable<IFormFile> pictures)
        {
            if (pictures.IsNullOrEmpty())
            {
                await this.productService.UpdateAsync(model);

                return this.RedirectToDetails(model.Id);
            }

            foreach (var picture in pictures)
            {
                var uniqueFileName = this.fileProcessor.GetUniqueFileName(picture.FileName);
                var stream = picture.OpenReadStream();
                /// Upload the new picture to S3:
                var pictureUrl = await this.amazonS3FileManager.UploadFileAsync(
                    this.awsFunctionOptions.BucketName,
                    stream,
                    uniqueFileName,
                    S3CannedACL.PublicRead);

                await this.pictureService.CreateAsync(pictureUrl, model.Id);
            }

            await this.productService.UpdateAsync(model);

            return this.RedirectToDetails(model.Id);
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> Delete(string id)
        {
            var product = await this.productService.GetAsync(id);
            foreach (var picture in product.Pictures)
            {
                await this.pictureService.RemoveAsync(picture.Id);
                await this.amazonS3FileManager
                    .RemoveFileByUrlAsync(this.awsFunctionOptions.BucketName, picture.Url);
            }

            await this.productService.RemoveAsync(id);

            return RedirectToHome();
        }

        [HttpGet]
        [ValidateQueryParameters]
        public async Task<IActionResult> RemovePicture(string id, string url, string productId)
        {
            await this.amazonS3FileManager.RemoveFileByUrlAsync(this.awsFunctionOptions.BucketName, url);

            await this.pictureService.RemoveAsync(id);

            return RedirectToDetails(productId);
        }

        private IActionResult RedirectToDetails(string id)
        {
            return RedirectToAction(nameof(this.Details), new { id });
        }
    }
}