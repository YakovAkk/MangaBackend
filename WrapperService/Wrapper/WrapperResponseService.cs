using System.Net;
using WrapperService.Model.InputModel;
using WrapperService.Model.ResponseModel;

namespace WrapperService.Wrapper;

public static class WrapperResponseService
{
    public static ResponseWrapModel Wrap(WrapInputModel inputModel)
    {
        if(inputModel == null || inputModel.Data == null || inputModel.Data.Count() == 0)
        {
            return new ResponseWrapModel()
            {
                Data = new object[0],
                StatusCode = HttpStatusCode.NotFound,
                ErrorMessage = "No data"
            };
        }

        return new ResponseWrapModel()
        {
            Data = inputModel.Data,
            StatusCode = HttpStatusCode.OK,
            ErrorMessage = ""
        };
    }
}
