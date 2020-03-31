
namespace SWI.AirView.Models
{
    public class Response
    {
        public string Message { get; set; }
        public string Status { get; set; }
        public dynamic Value { get; set; }
    }
    public class SignatureRequestDto
    {
        public long Id { get; set; }
        public string base64String { get; set; }
    }
}