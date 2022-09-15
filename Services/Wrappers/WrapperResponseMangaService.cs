using Data.Models;
using Services.Response;
using Services.StatusCode;
using Services.Wrappers.Base;

namespace Services.Wrappers
{
    public class WrapperResponseMangaService : IWrapperResponseService<ResponseModel, MangaModel>, IWrapperMangaService
    {
        public ResponseModel WrapTheResponseModel(MangaModel response)
        {
            var wrappedResponse = new ResponseModel();

            if (!string.IsNullOrEmpty(response.MessageWhatWrong))
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
        public ResponseModel WrapTheResponseListOfModels(IEnumerable<MangaModel> response)
        {
            var wrappedResponse = new ResponseModel();

            if (response.Count() == 0)
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
