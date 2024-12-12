namespace TotalHRInsight.Models
{
    public class ErrorDeleteModel
    {
        public string Message { get; set; }
        public string RequestId { get; set; }
        public string Details { get; set; }
        public bool ShowDetails => !string.IsNullOrEmpty(Details);
    }
}
