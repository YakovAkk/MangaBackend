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
        public async override Task<IList<MangaModel>> GetAllAsync()
        {
            var list = await _db.Mangas.AsNoTracking().Include(m => m.Genres).Include(m => m.PathToFoldersWithGlava).ToListAsync();

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

            var manga = await _db.Mangas.AsNoTracking().Include(m => m.Genres).Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Id == id);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = $"The manga with id = {id} isn't contained in the database!"
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
                    MessageWhatWrong = $"The manga {item.Name} is contained in the database!"
                };
            }

            item.MessageWhatWrong = "";

            var result = await _db.Mangas.AddAsync(item);

            if (result == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = $"The manga {item.Name} hasn't added in the database!"
                };

            }

            await _db.SaveChangesAsync();

            manga = await _db.Mangas.Include(m => m.Genres).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = $"The manga {item.Name} hasn't added in the database!"
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
                    MessageWhatWrong = $"The manga with id = {id} isn't contained in the database!"
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
                    MessageWhatWrong = $"The manga {item.Name} hasn't updated in the database!"
                };
            }

            await _db.SaveChangesAsync();

            var manga = await _db.Mangas.Include(m => m.Genres).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (manga == null)
            {
                return new MangaModel()
                {
                    MessageWhatWrong = $"The manga {item.Name} hasn't added in the database!"
                };
            }

            return manga;
        }
        public async override Task<IList<MangaModel>> AddRange(IList<MangaModel> items)
        {
            var result = new List<MangaModel>();

            if(items == null && !items.Any())
            {
                return new List<MangaModel>();
            }

            foreach (var item in items)
            {
                result.Add(await CreateAsync(item));              
            };

            return result;
        }
        public async override Task<IList<MangaModel>> GetCertainPage(int sizeOfPage, int page)
        {

            var list = await _db.Mangas.AsNoTracking().Include(m => m.Genres)
                .Include(m => m.PathToFoldersWithGlava).Skip(page * sizeOfPage).Take(sizeOfPage).ToListAsync();

            if (list == null)
            {
                return new List<MangaModel>();
            }

            return list;
        }
        public async override Task<IList<MangaModel>> GetAllFavoriteAsync()
        {
            var list = await _db.Mangas.Where(i => i.IsFavorite).ToListAsync();

            if (list == null)
            {
                return new List<MangaModel>();
            }

            return list;
        }
        public async override Task<MangaModel> AddToFavorite(string Id)
        {
            var manga = await GetByIdAsync(Id);

            if (!String.IsNullOrEmpty(manga.MessageWhatWrong))
            {
                return new MangaModel()
                {
                    MessageWhatWrong = manga.MessageWhatWrong
                };
            }

            manga.IsFavorite = true;

            await _db.SaveChangesAsync();

            return manga;
        }
        public async override Task<MangaModel> RemoveFavorite(string Id)
        {
            var manga = await GetByIdAsync(Id);

            if (!String.IsNullOrEmpty(manga.MessageWhatWrong))
            {
                return new MangaModel()
                {
                    MessageWhatWrong = manga.MessageWhatWrong
                };
            }

            manga.IsFavorite = false;

            await _db.SaveChangesAsync();

            return manga;
        }
    }
}
