using WrapperService.Response;
using WrapperService.StatusCode;

namespace WrapperService.Wrapper;

public static class WrapperResponseService
{
    public static ResponseModel WrapResponsErrorWithData(object response, string mess = "")
    {
        if (response == null)
        {
            return new ResponseModel()
            {
                data = Enumerable.Empty<object>,
                StatusCode = CodeStatus.ErrorWithData,
                ErrorMessage = mess
            };
        }

        return new ResponseModel()
        {
            data = response,
            StatusCode = CodeStatus.Successful,
            ErrorMessage = ""
        };
    }
    public static ResponseModel WrapResponseEmpty(object response, string mess = "")
    {
        if (response == null)
        {
            return new ResponseModel()
            {
                data = Enumerable.Empty<object>,
                StatusCode = CodeStatus.Empty,
                ErrorMessage = mess
            };
        }

        return new ResponseModel()
        {
            data = response,
            StatusCode = CodeStatus.Successful,
            ErrorMessage = ""
        };
    }
}
