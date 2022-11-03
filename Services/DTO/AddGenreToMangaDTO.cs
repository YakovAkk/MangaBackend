namespace Services.DTO;

public class AddGenreToMangaDTO
{
    public string MangaId { get; set; }
    public List<string> Genres_id { get; set; }

    public override string ToString()
    {
        return $"MangaId = {MangaId} Genres_id = {Genres_id}";
    }
}
