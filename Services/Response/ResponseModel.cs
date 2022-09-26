using AutoWrapper;
using Services.Response.Base;
using Services.StatusCode;

namespace Services.Response
{
    public class ResponseModel : IResponseModel
    {
        [AutoWrapperPropertyMap(Prop.Result)]
        public object Data { get; set; }

        [AutoWrapperPropertyMap(Prop.Message)]

        public string Message { get; set; }

        [AutoWrapperPropertyMap(Prop.StatusCode)]
        public CodeStatus StatusCode { get; set; }
    }
}
