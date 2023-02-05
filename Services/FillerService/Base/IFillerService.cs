using Services.Model.DTO;

namespace Services.FillerService.Base;

public interface IFillerService
{
    public Task<ResponseFillDTO> AddGenres();
    public Task<ResponseFillDTO> AddMangas();
    public Task<ResponseFillDTO> DeleteAll();
    public Task<ResponseFillDTO> AddAdmin();
}
