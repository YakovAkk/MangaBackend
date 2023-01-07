using WrapperService.StatusCode;

namespace WrapperService.Response;

public class ResponseModel
{
    public object data { get; set; }
    public string ErrorMessage { get; set; }
    public CodeStatus StatusCode { get; set; }

    public override string ToString()
    {
        return $"Status code is {(int)StatusCode} Error Message {ErrorMessage}";
    }
}
