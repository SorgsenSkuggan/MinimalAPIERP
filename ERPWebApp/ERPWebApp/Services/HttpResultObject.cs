using System.Net;

namespace ERPWebApp.Services{
    public class HttpResultObject<T>{
        public bool IsSuccess { get; set; }
        public HttpStatusCode HttpResultCode { get; set; }
        public T Result { get; set; }
    }
}
