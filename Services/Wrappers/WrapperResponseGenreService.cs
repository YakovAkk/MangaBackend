using Data.Models;
using Repositories.Models;
using Services.Response;
using Services.StatusCode;
using Services.Wrappers.Base;

namespace Services.Wrappers
{
    public class WrapperResponseGenreService : IWrapperResponseService<ResponseModel, GenreModel>, IWrapperGenreService
    {
        public ResponseModel WrapTheResponseModel(GenreModel response)
        {
            var wrappedResponse = new ResponseModel();

            if (!String.IsNullOrEmpty(response.MessageWhatWrong))
            {
                wrappedResponse.data = null;
                wrappedResponse.StatusCode = CodeStatus.Empty;
                wrappedResponse.ErrorMessage = response.MessageWhatWrong;

                return wrappedResponse;
            }

            wrappedResponse.data = response;
            wrappedResponse.StatusCode = CodeStatus.Successful;
            wrappedResponse.ErrorMessage = "";

            return wrappedResponse;
        }
        public ResponseModel WrapTheResponseListOfModels(IEnumerable<GenreModel> response)
        {
            var wrappedResponse = new ResponseModel();

            if (!response.Any())
            {
                wrappedResponse.data = null;
                wrappedResponse.StatusCode = CodeStatus.Empty;
                wrappedResponse.ErrorMessage = "The Database doesn't have any manga";

                return wrappedResponse;
            }

            wrappedResponse.data = response;
            wrappedResponse.StatusCode = CodeStatus.Successful;
            wrappedResponse.ErrorMessage = "";

            return wrappedResponse;
        }
    }
}
