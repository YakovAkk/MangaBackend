using AutoMapper;
using Data.Models;
using Repositories.Models;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.Services.Base;

namespace Services.Services
{
    public class GenreService : BaseService<GenreModel, GenreDTO>, IGenreService
    {
       
        public GenreService(IGenreRepository repository, IMapper mapper) : base(repository, mapper) 
        {
        }
        public override async Task<GenreModel> AddAsync(GenreDTO item)
        {
            var model = _mapper.Map<GenreModel>(item);

            return await _repository.CreateAsync(model);
        }
        private GenreModel ConverterModelDTOToModel(GenreDTO item)
        {
            return _mapper.Map<GenreModel>(item);
        }
        public override async Task<IList<GenreModel>> AddRange(IList<GenreModel> list)
        {
            return await _repository.AddRange(list);
        }
        public override async Task<IList<GenreModel>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }
        public override async Task<GenreModel> GetByIdAsync(string id)
        {
            return await _repository.GetByIdAsync(id);
        }
        public async override Task<GenreModel> UpdateAsync(GenreDTO item)
        {
            if(String.IsNullOrEmpty(item.Id))
            {
                return new GenreModel()
                {
                    MessageWhatWrong = "Id was null or empty"
                };
            }

            var genre = await _repository.GetByIdAsync(item.Id);

            if(!String.IsNullOrEmpty(genre.MessageWhatWrong))
            {
                return new GenreModel()
                {
                    MessageWhatWrong = genre.MessageWhatWrong
                };
            };

            return await _repository.UpdateAsync(_mapper.Map<GenreModel>(item));
        }
    }
}
