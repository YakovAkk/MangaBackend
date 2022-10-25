namespace Services.DTO;

public class AddGenreToMangaDTO
{
   public string MangaId { get; set; }
   public List<string> Genres_id { get; set; }
}
