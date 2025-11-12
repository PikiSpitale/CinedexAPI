using System.Net;

namespace proyecto_prog4.Utils
{
    public class HttpResponseError : Exception
    {
        public HttpStatusCode StatusCode { get; set; }
        public string Message { get; set; } = null!;

        public HttpResponseError(HttpStatusCode statusCode, string message) : base(message)
        {
            StatusCode = statusCode;
            Message = message;
        }
    }
}
