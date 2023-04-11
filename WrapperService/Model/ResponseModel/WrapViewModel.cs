using System.Net;

namespace WrapperService.Model.ResponseModel;

public class WrapViewModel
{
    public object Data { get; set; }
    public string ErrorMessage { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}
