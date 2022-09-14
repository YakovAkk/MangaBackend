using Services.DTO;

namespace Services.FillerService.Base
{
    public interface IFillerSwervice
    {
        public Task<ResponseFillDTO> AddGenres();
        public Task<ResponseFillDTO> AddMangas();
    }
}
