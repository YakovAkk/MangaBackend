using Services.Model.DTO;

namespace Services.Services.Base;

public interface IFillerService
{
    public Task<ResponseFillDTO> AddGenres();
    public Task<ResponseFillDTO> AddMangas();
    public Task<ResponseFillDTO> AddAdmin();
}
