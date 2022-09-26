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

            if (!String.IsNullOrEmpty(response.MessageWhatWrong))
            {
                wrappedResponse.Data = null;
                wrappedResponse.StatusCode = CodeStatus.Empty;
                wrappedResponse.Message = response.MessageWhatWrong;

                return wrappedResponse;
            }

            wrappedResponse.Data = response;
            wrappedResponse.StatusCode = CodeStatus.Successful;
            wrappedResponse.Message = "";

            return wrappedResponse;
        }
        public ResponseModel WrapTheResponseListOfModels(IEnumerable<MangaModel> response)
        {
            var wrappedResponse = new ResponseModel();

            if (!response.Any())
            {
                wrappedResponse.Data = null;
                wrappedResponse.StatusCode = CodeStatus.Empty;
                wrappedResponse.Message = "The Database doesn't have any manga";

                return wrappedResponse;
            }

            wrappedResponse.Data = response;
            wrappedResponse.StatusCode = CodeStatus.Successful;
            wrappedResponse.Message = "";

            return wrappedResponse;
        }
    }
}
