using System.Net;

namespace EventsAPI.Contracts.Responses
{
    public class ApiBaseResult
    {
        public required bool Success { get; set; }
        public required HttpStatusCode StatusCode { get; set; }
        public DateTime DateTime { get; set; } = DateTime.UtcNow;
        public string? Message { get; set; }
    }

    public class ApiResult : ApiBaseResult { }

    public class ApiResult<T> : ApiBaseResult
    {
        public required T? Data { get; set; }
    }

    public class ValidationApiResult : ApiBaseResult
    {
        public required Dictionary<string, IEnumerable<string>> Errors { get; set; }
    }
}
