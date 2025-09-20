using System.Net;

namespace ResponseFramework
{
    public interface IResponse<TResult>
    {
        bool IsSuccessful { get; set; }
        string? Message { get; set; }
        string? ErrorMessage { get; set; }
        TResult? Result { get; set; }
        HttpStatusCode HttpStatusCode { get; set; }
    }
}
