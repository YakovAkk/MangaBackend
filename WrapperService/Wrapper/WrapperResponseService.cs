using WrapperService.Model.InputModel;
using WrapperService.Model.ResponseModel;
using WrapperService.StatusCode;

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
                StatusCode = CodeStatus.Empty,
                ErrorMessage = "No data"
            };
        }

        return new ResponseWrapModel()
        {
            Data = inputModel.Data,
            StatusCode = CodeStatus.Successful,
            ErrorMessage = ""
        };
    }
}
