using Data.Entities;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.ExtensionMapper;
using Services.Services.Base;

namespace Services.Services
{
    public class GenreService : BaseService<GenreEntity, GenreDTO>, IGenreService
    {
        public GenreService(IGenreRepository repository) : base(repository) { }
        public override async Task<GenreEntity> AddAsync(GenreDTO item)
        {
            if (item == null)
            {
                throw new Exception("The item was null");
            }

            var model = item.toEntity();

            try
            {
                return await _repository.CreateAsync(model);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public override async Task<IList<GenreEntity>> AddRange(IList<GenreDTO> list)
        {
            if (list == null)
            {
                throw new Exception("list was null");
            }

            var listModels = new List<GenreEntity>();

            foreach (var item in list)
            {
                var genre = item.toEntity();

                listModels.Add(genre);
            }

            return await _repository.AddRange(listModels);
        }
        public override async Task<IList<GenreEntity>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public override async Task<GenreEntity> GetByIdAsync(string id)
        {
            if (String.IsNullOrEmpty(id))
            {
                throw new Exception("Id was null or empty");
            }

            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            
        }
        public async override Task<GenreEntity> UpdateAsync(GenreDTO item)
        {
            if (item == null)
            {
                throw new Exception("Item was null");
            }

            if (String.IsNullOrEmpty(item.Id))
            {
                throw new Exception("Id was null or empty");
            }

            try
            {
                var genre = await _repository.GetByIdAsync(item.Id);

                genre = item.toEntity();

                return await _repository.UpdateAsync(genre);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
