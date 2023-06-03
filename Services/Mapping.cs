using Data.Entities;
using Omu.ValueInjecter;
using Services.Model.DTO;
using Services.Model.ViewModel;

namespace Services
{
    public static class Mapping
    {
        private static readonly MapperInstance mapper;

        static Mapping()
        {
            mapper = new MapperInstance();

            mapper.AddMap<GenreInput, GenreEntity>((from) =>
            {
                var entity = new GenreEntity().InjectFrom(from) as GenreEntity;
                return entity;
            });

            mapper.AddMap<UserEntity, UserViewModel>((from) =>
            {
                var viewModel = new UserViewModel().InjectFrom(from) as UserViewModel;
                return viewModel;
            });

            mapper.AddMap<MangaEntity, MangaViewModel>((from) =>
            {
                var viewModel = new MangaViewModel().InjectFrom(from) as MangaViewModel;
                return viewModel;
            });

            mapper.AddMap<GlavaMangaEntity, GlavaMangaEntityViewModel>((from) =>
            {
                var viewModel = new GlavaMangaEntityViewModel().InjectFrom(from) as GlavaMangaEntityViewModel;
                viewModel.LinkToPictures = new List<string>();

                var fullUrl = from.LinkToFirstPicture.Split('/');
                var mainUrl = new string[6];

                for (int i = 0; i < fullUrl.Length - 1; i++)
                {
                    mainUrl[i] = fullUrl[i];
                }

                var relateUrl = fullUrl[6].Split('.');
                var mainUrlStr = string.Join('/', mainUrl);

                if(relateUrl[1] == "pdf")
                {
                    viewModel.LinkToPictures.Add(string.Join('/', fullUrl));
                }
                else
                {
                    for (int i = 1; i <= viewModel.NumberOfPictures; i++)
                    {
                        viewModel.LinkToPictures.Add($"{mainUrlStr}/{i}.{relateUrl[1]}");
                    }
                }

                return viewModel;
            });

        }

        public static TDest MapTo<TDest>(this object source)
        {
            return mapper.Map<TDest>(source);
        }
    }
}
