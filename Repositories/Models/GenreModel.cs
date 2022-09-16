using Data.Models;
using Repositories.Models.Base;

namespace Repositories.Models
{
    public class GenreModel : IModel
    {
        public string? Id { get; set; }
        public string Name { get; set; }
        public bool IsFavorite { get; set; }
        public virtual List<MangaEntity> Mangas { get; set; }
        public string MessageWhatWrong { get; set ; }

        public GenreModel()
        {
            Mangas = new List<MangaEntity>();
        }

    }
}
