using Data.Entities;
using Omu.ValueInjecter;
using Services.Model.InputModel;
using Services.Model.ViewModel;

namespace Services
{
    public static class Mapping
    {
        private static readonly MapperInstance mapper;

        static Mapping()
        {
            mapper = new MapperInstance();

            mapper.AddMap<GenreInputModel, GenreEntity>((from) =>
            {
                var entity = new GenreEntity().InjectFrom(from) as GenreEntity;
                return entity;
            });

            mapper.AddMap<UserEntity, UserViewModel>((from) =>
            {
                var viewModel = new UserViewModel().InjectFrom(from) as UserViewModel;
                return viewModel;
            });
            mapper.AddMap<UserRegistrationInputModel, UserEntity>((from) =>
            {
                var entity = new UserEntity().InjectFrom(from) as UserEntity;
                return entity;
            });

            mapper.AddMap<MangaEntity, MangaViewModel>((from) =>
            {
                var viewModel = new MangaViewModel().InjectFrom(from) as MangaViewModel;
                return viewModel;
            });
            mapper.AddMap<MangaInputModel, MangaEntity>((from) =>
            {
                var entity = new MangaEntity().InjectFrom(from) as MangaEntity;
                return entity;
            });

            mapper.AddMap<GlavaMangaEntity, GlavaMangaEntityViewModel>((from) =>
            {
                var viewModel = new GlavaMangaEntityViewModel().InjectFrom(from) as GlavaMangaEntityViewModel;
                return viewModel;
            });

            mapper.AddMap<RememberReadingItemInputModel, RememberReadingItem>((from) =>
            {
                var viewModel = new RememberReadingItem().InjectFrom(from) as RememberReadingItem;
                return viewModel;
            });
            mapper.AddMap<RememberReadingItem, RememberReadingItemViewModel>((from) =>
            {
                var viewModel = new RememberReadingItemViewModel().InjectFrom(from) as RememberReadingItemViewModel;
                return viewModel;
            });
        }

        public static TDest MapTo<TDest>(this object source)
        {
            return mapper.Map<TDest>(source);
        }
    }
}
