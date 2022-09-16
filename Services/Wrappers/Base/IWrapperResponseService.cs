using Data.Models.Base;
using Repositories.Models.Base;
using Services.Response.Base;

namespace Services.Wrappers.Base
{
    public interface IWrapperResponseService<TR, TI>
        where TR : IResponseModel
        where TI : IModel
    {
        TR WrapTheResponseModel(TI response);
        TR WrapTheResponseListOfModels(IEnumerable<TI> response);
    }
}
