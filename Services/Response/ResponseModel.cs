using Services.Response.Base;
using Services.StatusCode;

namespace Services.Response;

public class ResponseModel : IResponseModel
{
    public object data { get; set; }
    public string ErrorMessage { get; set; }
    public CodeStatus StatusCode { get; set; }
}
