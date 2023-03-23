namespace Data.Entities
{
    public partial class MangaEntity
    {
        public void ClearGenre()
        {
            Genres = null;
        }

        public void ClearPathToFoldersWithGlava()
        {
            PathToFoldersWithGlava = null;
        }
    }
}
