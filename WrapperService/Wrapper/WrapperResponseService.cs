using System.Net;
using WrapperService.Model.ResponseModel;

namespace WrapperService.Wrapper;

public static class WrapperResponseService
{
    public static WrapViewModel Wrap<T>(T inputModel = null, string errorMessage = null) where T : class
    {
        if(errorMessage is not null)
        {
            return new WrapViewModel()
            {
                Data = new object[0],
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = errorMessage
            };
        }

        if (inputModel is null)
        {
            return new WrapViewModel()
            {
                Data = new object[0],
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = "No data"
            };
        }

        if (typeof(T) == typeof(IEnumerable<object>))
        {
            var result = (IEnumerable<object>)inputModel;

            if (!result.Any())
            {
                return new WrapViewModel()
                {
                    Data = new object[0],
                    StatusCode = HttpStatusCode.NotFound,
                    ErrorMessage = "No data"
                };
            }
        }

        return new WrapViewModel()
        {
            Data = inputModel,
            StatusCode = HttpStatusCode.OK,
            ErrorMessage = ""
        };
    }
}
