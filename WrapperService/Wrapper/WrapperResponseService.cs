using WrapperService.Response;
using WrapperService.StatusCode;

namespace WrapperService.Wrapper;

public static class WrapperResponseService
{
    public static ResponseModel WrapResponsErrorWithData(object response, string mess = "")
    {
        return new ResponseModel()
        {
            Data = Enumerable.Empty<object>,
            StatusCode = CodeStatus.ErrorWithData,
            ErrorMessage = mess
        };
    }
    public static ResponseModel WrapResponseEmpty(object response, string mess = "")
    {
        return new ResponseModel()
        {
            Data = Enumerable.Empty<object>,
            StatusCode = CodeStatus.Empty,
            ErrorMessage = mess
        };
    }
}
