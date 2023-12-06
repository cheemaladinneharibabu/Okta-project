namespace oktaMFA.Models
{
    public class ErrorResponse
    {
        public string ErrorCode { get; set; }
        public string ErrorSummary { get; set; }
        public List<ErrorCause> ErrorCauses { get; set; }
    }

    public class ErrorCause
    {
        public string ErrorSummary { get; set; }
    }
}
