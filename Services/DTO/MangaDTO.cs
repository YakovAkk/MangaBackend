using Data.Models;
using Services.DTO.Base;

namespace Services.DTO
{
    public class MangaDTO : IModelDTO
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public string PathToTitlePicture { get; set; }
        public List<GlavaMangaModel> PathToFoldersWithGlava { get; set; }
        public string Description { get; set; }
        public List<string> genres_id { get; set; }

    }
}
