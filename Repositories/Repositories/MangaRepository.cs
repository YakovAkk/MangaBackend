using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories
{
    public class MangaRepository : BaseRepository<MangaEntity>, IMangaRepository
    {
        public MangaRepository(AppDBContent db) : base(db)
        {

        }
        public async override Task<IList<MangaEntity>> GetAllAsync()
        {
            var list = await _db.Mangas.Include(m => m.Genres).AsNoTracking().Include(m => m.PathToFoldersWithGlava).ToListAsync();

            if (list == null)
            {
                return new List<MangaEntity>();
            }

            return list;
        }
        public async override Task<MangaEntity> DeleteAsync(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                throw new Exception("Id was null or empty");
            }

            var manga = await _db.Mangas.Include(m => m.Genres)
                .AsNoTracking().Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Id == id);

            if (manga == null)
            {
                throw new Exception($"The manga with id = {id} isn't contained in the database!");
            }

            _db.Mangas.Remove(manga);

            await _db.SaveChangesAsync();

            return manga;
        }
        public async override Task<MangaEntity> CreateAsync(MangaEntity item)
        {
            if (item == null)
            {
                throw new Exception("The item was null");
            }

            var manga = await _db.Mangas.FirstOrDefaultAsync(i => i.Name == item.Name);

            if (manga != null)
            {
                throw new Exception($"The manga {item.Name} is contained in the database!");
            }

            var result = await _db.Mangas.AddAsync(item);

            if (result == null)
            {
                throw new Exception($"The manga {item.Name} hasn't added in the database!");
            }

            await _db.SaveChangesAsync();

            manga = await _db.Mangas.Include(m => m.Genres)
                .AsNoTracking().Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (manga == null)
            {
                throw new Exception($"The manga {item.Name} hasn't added in the database!");
            }

            return manga;
        }
        public async override Task<MangaEntity> GetByIdAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new Exception("Id was null or empty");
            }

            var manga = await _db.Mangas.Include(m => m.Genres)
                .Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Id == id);

            if (manga == null)
            {
                throw new Exception($"The manga with id = {id} isn't contained in the database!");
            }

            return manga;
        }
        public async override Task<MangaEntity> UpdateAsync(MangaEntity item)
        {
            if (item == null)
            {
                throw new Exception("Item was null");
            }

            var result = _db.Mangas.Update(item);

            if (result == null)
            {
                throw new Exception($"The manga {item.Name} hasn't updated in the database!");
            }

            await _db.SaveChangesAsync();

            var manga = await _db.Mangas.Include(m => m.Genres).AsNoTracking().Include(m => m.PathToFoldersWithGlava).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (manga == null)
            {
                throw new Exception($"The manga {item.Name} hasn't added in the database!");
            }

            return manga;
        }
        public async override Task<IList<MangaEntity>> AddRange(IList<MangaEntity> items)
        {
            var result = new List<MangaEntity>();

            if(items == null && !items.Any())
            {
                return result;
            }

            foreach (var item in items)
            {
                try
                {
                    var model = await CreateAsync(item);
                    result.Add(model);
                }
                catch (Exception)
                {
                    continue;
                }
     
            };

            return result;
        }
        public async override Task<IList<MangaEntity>> GetCertainPage(int sizeOfPage, int page)
        {

            var list = await _db.Mangas.Include(m => m.Genres).AsNoTracking()
                .Include(m => m.PathToFoldersWithGlava).Skip((page -1) * sizeOfPage).Take(sizeOfPage).ToListAsync();

            if (list == null)
            {
                return new List<MangaEntity>();
            }

            return list;
        }
        public async override Task<IList<MangaEntity>> GetAllFavoriteAsync()
        {
            var list = await _db.Mangas.Where(i => i.IsFavorite).ToListAsync();

            if (list == null)
            {
                return new List<MangaEntity>();
            }

            return list;
        }
        public async override Task<MangaEntity> AddToFavorite(string Id)
        {
            try
            {
                var manga = await GetByIdAsync(Id);

                manga.IsFavorite = true;

                await UpdateAsync(manga);

                await _db.SaveChangesAsync();

                return await GetByIdAsync(manga.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async override Task<MangaEntity> RemoveFavorite(string Id)
        {
            try
            {
                var manga = await GetByIdAsync(Id);

                manga.IsFavorite = false;

                await UpdateAsync(manga);

                await _db.SaveChangesAsync();

                return await GetByIdAsync(manga.Id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        public async override Task<IList<MangaEntity>> FiltrationByName(string name)
        {
            if (String.IsNullOrEmpty(name))
            {
                return new List<MangaEntity>();
            }

            var result = await _db.Mangas.Include(i => i.Genres).Include(i=> i.PathToFoldersWithGlava).Where(i => i.Name.ToLower().Contains(name.ToLower())).ToListAsync();

            if (!result.Any())
            {
                return new List<MangaEntity>();
            }

            return result;
        }
        public async Task<List<MangaEntity>> FiltrationByDate(int year)
        {
            var result = await _db.Mangas.Include(i => i.Genres).Include(i => i.PathToFoldersWithGlava).Where(i => i.ReleaseYear > year).ToListAsync();

            if(result == null)
            {
                return new List<MangaEntity>();
            }

            return result;
        }
    }
}
