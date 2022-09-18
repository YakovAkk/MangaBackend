using AutoMapper;
using Data.Models;
using Repositories.Models;
using Services.DTO;

namespace MangaBackend.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<GenreEntity, GenreModel>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.IsFavorite, opt => opt.MapFrom(src => src.IsFavorite))
            .ForMember(dst => dst.Mangas, opt => opt.MapFrom(src => src.Mangas))
            .ForMember(dst => dst.MessageWhatWrong, opt => opt.MapFrom(src => ""));

            CreateMap<GenreModel, GenreEntity>()
            .ForMember(dst => dst.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dst => dst.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dst => dst.IsFavorite, opt => opt.MapFrom(src => src.IsFavorite))
            .ForMember(dst => dst.Mangas, opt => opt.MapFrom(src => src.Mangas));


            //CreateMap<List<GenreModel>, List<GenreEntity>>();
            //CreateMap<List<GenreEntity>, List<GenreModel>>();

            CreateMap<GenreDTO, GenreModel>();
            CreateMap<GenreModel, GenreDTO>();

            CreateMap<MangaModel, MangaEntity>();
            CreateMap<MangaEntity, MangaModel> ();

            CreateMap<MangaDTO, MangaModel>();
            CreateMap<MangaModel, MangaDTO>();

        }
    }
}
