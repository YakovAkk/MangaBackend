using AutoWrapper;
using Services.StatusCode;

namespace MangaBackend.ResponceModelForWrapper
{
    public class Response
    {
        public object Data { get; set; }
    }

    public class ErrorResponse
    {
        [AutoWrapperPropertyMap(Prop.ResponseException)]
        public string ErrorMessage { get; set; }
    }
}
