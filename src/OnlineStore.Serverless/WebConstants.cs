namespace OnlineStore.Serverless
{
    public class WebConstants
    {
        public const string AdministratorEmail = "administrator@online-store.com";
        public const string AdministratorName = "Administrator";
        public const string AdminAreaName = "admin";

        public const string AnonymounsBrowserCookieName = "anonymousBrowserId";

        public const string PaginationPartialPath = @"~/Views/Shared/_PaginationElementsPartial.cshtml";
        public const string SuccessfullRegistrationEmailTitle = "Registration successfull";
        public const string SuccessfullRegistrationEmailContent = "Congratulations!{0}. " +
            "You have successfully registered with email: {1}.{0}" +
            "Enjoy our website.";

        public const string UserAreaName = "User";
        public const int UsersCountPerPage = 20;
        public const int OrdersCountPerPage = 20;
        public const int ProductsCountPerPage = 21;
        public const int SearchResultItemsCountPerPage = 10;
    }
}
