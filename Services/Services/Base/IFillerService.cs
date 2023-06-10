using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services.Services.Base;

public interface IFillerService
{
    Task<ResponseViewModel> AddGenres();
    Task<ResponseViewModel> AddMangas();
    Task<ResponseViewModel> AddAdmin();
    Task<ResponseViewModel> DeleteUser(UserInputModel user);
}
