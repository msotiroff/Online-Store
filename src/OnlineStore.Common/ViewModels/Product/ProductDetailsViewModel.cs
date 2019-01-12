using OnlineStore.Common.ViewModels.Picture;
using System.Collections.Generic;

namespace OnlineStore.Common.ViewModels.Product
{
    public class ProductDetailsViewModel
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int Availability { get; set; }

        public string CategoryName { get; set; }

        public IEnumerable<PictureConciseViewModel> Pictures { get; set; }
    }
}
