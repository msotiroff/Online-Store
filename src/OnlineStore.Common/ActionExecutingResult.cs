namespace OnlineStore.Common
{
    public class ActionExecutingResult
    {
        public ActionExecutingResult(bool success)
        {
            this.Success = success;
        }

        public ActionExecutingResult(string errorMessage) 
            : this(false)
        {
            this.ErrorMessage = errorMessage;
        }

        public bool Success { get; set; }

        public string ErrorMessage { get; set; }
    }
}
