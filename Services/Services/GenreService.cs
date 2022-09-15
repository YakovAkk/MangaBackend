﻿using Data.Models;
using Repositories.Repositories.Base;
using Services.DTO;
using Services.Services.Base;

namespace Services.Services
{
    public class GenreService : BaseService<GenreModel, GenreDTO>, IGenreService
    {
        public GenreService(IGenreRepository repository) : base(repository) { }
        public override async Task<GenreModel> AddAsync(GenreDTO item)
        {
            var model = new GenreModel()
            {
                Name = item.Name,
                MessageWhatWrong = ""
            };

            return await _repository.CreateAsync(model);
        }

        private GenreModel ConverterModelDTOToModel(GenreDTO item)
        {
            var model = new GenreModel()
            {
                Name = item.Name,
                MessageWhatWrong = ""
            };

            return model;
        }
        public override async Task<IList<GenreModel>> AddRange(IList<GenreDTO> list)
        {
            var listModels = new List<GenreModel>();

            foreach (var item in list)
            {
                var genre = ConverterModelDTOToModel(item);

                listModels.Add(genre);
            }

            return await _repository.AddRange(listModels);
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

            genre = ConverterModelDTOToModel(item);

            return await _repository.UpdateAsync(genre);
        }
    }
}
