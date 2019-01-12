using System.Collections.Generic;

namespace OnlineStore.Common.ViewModels.Category
{
    public class CategoryDeleteViewModel
    {
        public string Id { get; set; }

        public string PictureUrl { get; set; }

        public IEnumerable<string> ProductsIds { get; set; }
    }
}
