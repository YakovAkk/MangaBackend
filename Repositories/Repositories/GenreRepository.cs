using Data.Database;
using Data.Models;
using Microsoft.EntityFrameworkCore;
using Repositories.Repositories.Base;

namespace Repositories.Repositories
{
    public class GenreRepository : BaseRepository<GenreModel>, IGenreRepository
    {

        private readonly IMangaRepository _mangaRepository;

        public GenreRepository(AppDBContent db, IMangaRepository mangaRepository) : base(db)
        {
            _mangaRepository = mangaRepository;
        }
        public async override Task<IList<GenreModel>> GetAllAsync()
        {
            var list = await _db.Genres.ToListAsync();

            if (list == null)
            {
                return new List<GenreModel>();
            }

            return list;
        }
        public async override Task<GenreModel> GetByIdAsync(string id)
        {
            if (id == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = "Id was null"
                };
            }

            var genre = await _db.Genres.Include(m => m.Mangas).FirstOrDefaultAsync(i => i.Id == id);

            if (genre == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre with id = {id} isn't contained in the database!"
                };
            }

            return genre;
        }
        public async override Task<GenreModel> CreateAsync(GenreModel item)
        {
            if (item == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The Item was null"
                };
            }

            var genre = await _db.Genres.FirstOrDefaultAsync(i => i.Name == item.Name);

            if (genre != null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre {item.Name} is contained in the database!"
                };
            }

            var result = await _db.Genres.AddAsync(item);

            if (result == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre {item.Name} hasn't added in the database!"
                };
            }

            await _db.SaveChangesAsync();

            genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (genre == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre {item.Name} hasn't added in the database!"
                };
            }

            return genre;
        }
        public async override Task<GenreModel> DeleteAsync(string id)
        {
            if (id == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = "Id was null"
                };
            }

            var genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Id == id);

            if (genre == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre with id = {id} isn't contained in the database!"
                };
            }

            _db.Genres.Remove(genre);

            var mangaList = await _mangaRepository.GetAllAsync();

            mangaList.AsParallel().ForAll(m => m.Genres.Remove(genre));

            await _db.SaveChangesAsync();

            return genre;
        }
        public async override Task<GenreModel> UpdateAsync(GenreModel item)
        {
            if (item == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = "Item was null"
                };
            }

            var result = _db.Genres.Update(item);

            if(result == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre {item.Name} wan't updated"
                };
            }

            await _db.SaveChangesAsync();

            var genre = await _db.Genres.Include(g => g.Mangas).FirstOrDefaultAsync(i => i.Name == item.Name);

            if (genre == null)
            {
                return new GenreModel()
                {
                    MessageWhatWrong = $"The genre {item.Name} hasn't updated in the database!"
                };
            }

            return genre;
        }
        public async override Task<IList<GenreModel>> AddRange(IList<GenreModel> items)
        {
            var result = new List<GenreModel>();

            if (items == null && !items.Any())
            {
                return result;
            }

            foreach (var item in items)
            {
                var model = await CreateAsync(item);

                if (string.IsNullOrEmpty(model.MessageWhatWrong))
                {
                    result.Add(model);
                }
            };

            return result;
        }
        public async override Task<IList<GenreModel>> GetCertainPage(int sizeOfPage, int page)
        {
            var list = await _db.Genres.Skip(page * sizeOfPage).Take(sizeOfPage).ToListAsync();

            if (list == null)
            {
                return new List<GenreModel>();
            }

            return list;
        }
        public async override Task<IList<GenreModel>> GetAllFavoriteAsync()
        {
            var list = await _db.Genres.Where(i => i.IsFavorite).ToListAsync();

            if (list == null)
            {
                return new List<GenreModel>();
            }

            return list;
        }
        public async override Task<GenreModel> AddToFavorite(string Id)
        {
            var genre = await GetByIdAsync(Id);

            if (!String.IsNullOrEmpty(genre.MessageWhatWrong))
            {
                return new GenreModel()
                {
                    MessageWhatWrong = genre.MessageWhatWrong
                };
            }

            genre.IsFavorite = true;

            await _db.SaveChangesAsync();

            return genre;
        }
        public async override Task<GenreModel> RemoveFavorite(string Id)
        {
            var genre = await GetByIdAsync(Id);

            if (!String.IsNullOrEmpty(genre.MessageWhatWrong))
            {
                return new GenreModel()
                {
                    MessageWhatWrong = genre.MessageWhatWrong
                };
            }

            genre.IsFavorite = false;

            await _db.SaveChangesAsync();

            return genre;
        }
    }
}
