﻿using Data.Models;
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
        public ResponseModel WrapTheResponseListOfModels(IEnumerable<GenreModel> response)
        {
            var wrappedResponse = new ResponseModel();

            if (!response.Any())
            {
                wrappedResponse.Data = null;
                wrappedResponse.StatusCode = CodeStatus.Empty;
                wrappedResponse.Message = "The Database doesn't have any genre";

                return wrappedResponse;
            }

            wrappedResponse.Data = response;
            wrappedResponse.StatusCode = CodeStatus.Successful;
            wrappedResponse.Message = "";

            return wrappedResponse;
        }
    }
}
