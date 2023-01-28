using Data.Entities;
using Services.Model.DTO.Base;

namespace Services.Model.DTO;

public class MangaDTO : IModelDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }
    public string PathToTitlePicture { get; set; }
    public int ReleaseYear { get; set; }
    public string AgeRating { get; set; }
    public int NumbetOfChapters { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public List<GlavaMangaEntity> PathToFoldersWithGlava { get; set; }
    public List<string> genres_id { get; set; }

    public MangaDTO()
    {
        PathToFoldersWithGlava = new List<GlavaMangaEntity>();
        genres_id = new List<string>();
    }

}
