using Data.Entities;
using Services.Response;
using Services.StatusCode;
using Services.Wrappers.Base;

namespace Services.Wrappers;

public class WrapperUserService : IWrapperResponseService<ResponseModel, UserEntity>, IWrapperUserService
{
    public ResponseModel WrapTheResponseListOfModels(IEnumerable<UserEntity> response)
    {
        var wrappedResponse = new ResponseModel();

        if (!response.Any())
        {
            wrappedResponse.data = new List<MangaEntity>();
            wrappedResponse.StatusCode = CodeStatus.Empty;
            wrappedResponse.ErrorMessage = "The Database doesn't have any manga";

            return wrappedResponse;
        }

        wrappedResponse.data = response;
        wrappedResponse.StatusCode = CodeStatus.Successful;
        wrappedResponse.ErrorMessage = "";

        return wrappedResponse;
    }

    public ResponseModel WrapTheResponseModel(UserEntity response, string mess = "")
    {
        if (response == null)
        {
            return new ResponseModel()
            {
                data = new GenreEntity(),
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
