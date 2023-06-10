using Data.Entities;

namespace Services.Model.InputModel;

public class MangaInputModel
{
    public string Name { get; set; }
    public string PathToTitlePicture { get; set; }
    public int ReleaseYear { get; set; }
    public string AgeRating { get; set; }
    public int NumbetOfChapters { get; set; }
    public string Author { get; set; }
    public string Description { get; set; }
    public List<GlavaMangaEntity> PathToFoldersWithGlava { get; set; }
    public List<int> Genres_Ids { get; set; }

    public MangaInputModel()
    {
        PathToFoldersWithGlava = new List<GlavaMangaEntity>();
        Genres_Ids = new List<int>();
    }

}
