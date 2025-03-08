namespace LoggingAPI.Models
{
    public class Result
    {
        public bool IsSuccess { get; set; }
        public bool IsFailure { get; set; }
        public string Error { get; set; } = String.Empty;
    }
}
