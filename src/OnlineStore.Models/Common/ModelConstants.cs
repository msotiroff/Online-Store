namespace OnlineStore.Models.Common
{
    public class ModelConstants
    {
        public const string CategoryListPath = @"OnlineStore.Models\Common\CategoryList.json";
        public const int CategoryNameMinLength = 3;

        public const int UsernameMinLength = 3;
        public const int UsernameMaxLength = 30;
        public const string UserPasswordPattern = @"^(?=.*[A-Za-z])(?=.*\d)[A-Za-z\d]{4,}$";
        public const string UserPasswordErrorMsg = "Password must be minimum four characters, at least one letter and one number";
        public const int UserNameMinLength = 2;

        public const int ProductNameMinLength = 3;
        public const int ProductDescriptionMinLength = 5;
        public const string ProductPriceMinValueAsString = "0.01";
        public const string ProductPriceMaxValueAsString = "1000000.0";
    }
}
