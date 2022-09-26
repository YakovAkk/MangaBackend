using Data.Models.Base;
using Services.Response.Base;

namespace Services.Wrappers.Base
{
    public interface IWrapperResponseService<TR, TI>
        where TR : IResponseModel
        where TI : IModel
    {
        TR WrapTheResponseModel(TI response, string mess = "");
        TR WrapTheResponseListOfModels(IEnumerable<TI> response);
    }
}
