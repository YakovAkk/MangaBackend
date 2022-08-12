using Data.Database;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories
{
    public class MangaRepository : BaseRepository<MangaModel>, IMangaRepository
    {
        public MangaRepository(AppDBContent db) : base(db)
        {

        }
        public override async Task<List<MangaModel>> GetAllAsync()
        {
            var list = await _db.Mangas.AsNoTracking().Include(m => m.Genres).ToListAsync();

            if (list == null)
            {
                return new List<MangaModel>();
            }

            return list;
        }
        public async override Task<MangaModel> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "Id was null"
                };
            }

            var manga = await _db.Mangas.AsNoTracking().Include(m => m.Genres).FirstOrDefaultAsync(i => i.Id == id);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga isn't contained in the database!"
                };
            }

            _db.Mangas.Remove(manga);

            await _db.SaveChangesAsync();

            return manga;
        }
        public async override Task<MangaModel> CreateAsync(MangaModel item)
        {
            if (item == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "Item was null"
                };
            }

            var manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Id == item.Id);

            if (manga != null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga is contained in the database!"
                };
            }

            var addedItem = new MangaModel()
            {
                Name = item.Name,
                PathToTitlePicture = item.PathToTitlePicture,
                Description = item.Description,
                Genres = item.Genres,
                PathToFoldersWithGlava = item.PathToFoldersWithGlava,
                MessageWhatWrong = ""
            };

            var result = await _db.Mangas.AddAsync(addedItem);

            if (result == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga hasn't added in the database!"
                };

            }

            await _db.SaveChangesAsync();

            manga = await _db.Mangas.Include(m => m.Genres).FirstOrDefaultAsync(i => i.Name == addedItem.Name);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga hasn't added in the database!"
                };
            }

            return manga;
        }
        public async override Task<MangaModel> GetByIdAsync(string id)
        {
            if (id == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "Id was null"
                };
            }

            var manga = await _db.Mangas.Include(m => m.Genres).FirstOrDefaultAsync(i => i.Id == id);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga isn't contained in the database!"
                };
            }

            return manga;
        }
        public async override Task<MangaModel> UpdateAsync(MangaModel item)
        {
            if (item == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "Item was null"
                };
            }

            var result = _db.Mangas.Update(item);

            if (result == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The genre hasn't updated in the database!"
                };
            }

            await _db.SaveChangesAsync();

            var manga = await _db.Mangas.Include(m => m.Genres).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = "The manga hasn't added in the database!"
                };
            }

            return manga;
        }
    }
}
