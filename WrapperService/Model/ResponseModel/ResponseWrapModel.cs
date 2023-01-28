using WrapperService.StatusCode;

namespace WrapperService.Model.ResponseModel;

public class ResponseWrapModel
{
    public object Data { get; set; }
    public string ErrorMessage { get; set; }
    public CodeStatus StatusCode { get; set; }

    public override string ToString()
    {
        return $"Status code is {(int)StatusCode} Error Message {ErrorMessage}";
    }
}
