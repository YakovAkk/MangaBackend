using Services.Model.ViewModel;

namespace Services.Services.Base;

public interface IFillerService
{
    public Task<ResponseViewModel> AddGenres();
    public Task<ResponseViewModel> AddMangas();
    public Task<ResponseViewModel> AddAdmin();
}
