using Data.Entities;
using Services.FillerService.Base;
using Services.Model.DTO;
using Services.Services.Base;

namespace Services.FillerService;

public class FillerService : IFillerService
{
    private readonly IMangaService _mangaService;
    private readonly IGenreService _genreService;
    private readonly IAuthService _authService;
    public FillerService(IMangaService mangaService, IGenreService genreService, IAuthService authService)
    {
        _mangaService = mangaService;
        _genreService = genreService;
        _authService = authService;
    }

    public async Task<ResponseFillDTO> AddGenres()
    {
        var listOfGenres = new List<GenreInput>()
        {
            new GenreInput("Action"),
            new GenreInput("Romance"),
            new GenreInput("Comedy"),
            new GenreInput("Drama"),
            new GenreInput("Fantasy"),
            new GenreInput("Everyday life"),
            new GenreInput("Adventures"),
            new GenreInput("Art"),
            new GenreInput("Madness"),
            new GenreInput("Action movie"),
            new GenreInput("Military"),
            new GenreInput("Harem"),
            new GenreInput("Gender intrigue"),
            new GenreInput("Heroic fantasy"),
            new GenreInput("Demons"),
            new GenreInput("Detective"),
            new GenreInput("Children's"),
            new GenreInput("Josei"),
            new GenreInput("The game"),
            new GenreInput("Isekai"),
            new GenreInput("Story"),
            new GenreInput("Cyberpunk"),
            new GenreInput("Kodomo"),
            new GenreInput("Space"),
            new GenreInput("Magic"),
            new GenreInput("Maho-shoujo"),
            new GenreInput("Cars"),
            new GenreInput("Fur"),
            new GenreInput("Mystic"),
            new GenreInput("Music"),
            new GenreInput("Science fiction"),
            new GenreInput("Omegaverse"),
            new GenreInput("Parody"),
            new GenreInput("Police"),
            new GenreInput("Post-apocalyptic"),
            new GenreInput("Psychology"),
            new GenreInput("Samurai fighter"),
            new GenreInput("Supernatural"),
            new GenreInput("Shojo"),
            new GenreInput("Shojo-ai"),
            new GenreInput("Shonen"),
            new GenreInput("Shonen Ai"),
            new GenreInput("Sport"),
            new GenreInput("Superpower"),
            new GenreInput("Seinen"),
            new GenreInput("Tragedy"),
            new GenreInput("Thriller"),
            new GenreInput("Horror"),
            new GenreInput("Fiction"),
            new GenreInput("Schlola"),
            new GenreInput("Erotica"),
            new GenreInput("Ecchi"),
            new GenreInput("Yuri"),
            new GenreInput("Yaoi")
        };

        try
        {
            var resultGenre = await _genreService.AddRange(listOfGenres);

            if (!resultGenre.Any())
            {
                return new ResponseFillDTO()
                {
                    IsSuccess = false,
                    MessageWhatWrong = "The Ganres is already conteited in the database"
                };
            }

            return new ResponseFillDTO()
            {
                IsSuccess = true,
                MessageWhatWrong = ""
            };
        }
        catch (Exception ex)
        {
            return new ResponseFillDTO()
            {
                IsSuccess = false,
                MessageWhatWrong = ex.Message
            };
        }
    }
    public async Task<ResponseFillDTO> AddMangas()
    {
        var genres = await _genreService.GetAllAsync();

        if (!genres.Any())
        {
            return new ResponseFillDTO()
            {
                IsSuccess = false,
                MessageWhatWrong = "The Database doesn't have any genres"
            };
        }

        var mangas = new List<MangaInput>();
        mangas.Add(CreateAttackOfTheTitansManga(genres));
        mangas.Add(CreateNarutoManga(genres));
        mangas.Add(CreateSevenDeadlySinsManga(genres));
        mangas.Add(CreateTokyoGhoulSinsManga(genres));

        try
        {
            var resultManga = await _mangaService.AddRange(mangas);

            if (!resultManga.Any())
            {
                return new ResponseFillDTO()
                {
                    IsSuccess = false,
                    MessageWhatWrong = "The Mangas is already conteited in the database"
                };
            }

            return new ResponseFillDTO()
            {
                IsSuccess = true,
                MessageWhatWrong = ""
            };
        }
        catch (Exception ex)
        {
            return new ResponseFillDTO()
            {
                IsSuccess = true,
                MessageWhatWrong = ex.Message
            };
        }  
    }
    public async Task<ResponseFillDTO> AddAdmin()
    {
        var admin = new UserRegistrationDTO()
        {
           UserName = "admin",
           Email = "admin@gmail.com",
           Password = "pa$$w0rd1",
           ConfirmPassword = "pa$$w0rd1"
        };

        var response = await _authService.RegisterAsync(admin);

        if(response is null)
        {
            return new ResponseFillDTO()
            {
                IsSuccess = false,
                MessageWhatWrong = "Error"
            };
        }

        return new ResponseFillDTO()
        {
            IsSuccess = true,
            MessageWhatWrong = ""
        };
    }
    private MangaInput CreateAttackOfTheTitansManga(IList<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Action",
           "Drama",
           "Adventures",
           "Psychology",
           "Shounen",
           "Tragedy",
           "Horror",
           "Fantasy",
           "Post-apocalyptic",
           "Monsters",
           "Survival"
        };

        var genres_id = new List<string>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/attackofthetitans/glava1/1.jpg",
                NumberOfPictures = 54
            },
            new GlavaMangaEntity()
            {
                NumberOfGlava = 2,
                LinkToFirstPicture = "manga/attackofthetitans/glava2/1.jpg",
                NumberOfPictures = 43
            },
            new GlavaMangaEntity()
            {
                NumberOfGlava = 3,
                LinkToFirstPicture = "manga/attackofthetitans/glava3/1.jpg",
                NumberOfPictures = 46
            },
            new GlavaMangaEntity()
            {
                NumberOfGlava = 4,
                LinkToFirstPicture = "manga/attackofthetitans/glava4/1.jpg",
                NumberOfPictures = 49
            },
        };

        return new MangaInput()
        {
            Name = "Attack of the Titans",
            PathToTitlePicture = "manga/attackofthetitans/titleimage.jpg",
            Genres_names = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Давным-давно человечество было всего лишь «их» кормом, до тех пор, пока оно не построило гигантскую стену вокруг своей страны. С тех пор прошло сто лет мира и большинство людей жили счастливой, беззаботной жизнью. Но за долгие годы спокойствия пришлось заплатить огромную цену, и в 845 году они снова познали чувство ужаса и беспомощности – стена, которая была их единственным спасением, пала. «Они» прорвались. Половина человечества съедена, треть территории навсегда потеряна...",
            NumbetOfChapters = 140,
            AgeRating = "18+",
            Author = "ISAYAMA Hajime",
            ReleaseYear = 2009
        };
    }
    private MangaInput CreateNarutoManga(IList<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Action",
           "Martial arts",
           "Drama",
           "Comedy",
           "Adventures",
           "Psychology",
           "Supernatural",
           "Shounen",
           "Sword fighting",
           "War",
           "GG man",
           "Friendship",
           "Revenge",
           "Saving the world",
           "Teacher / student",
           "Ghosts / Spirits",
           "Cruel world"
        };

        var genres_id = new List<string>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 0,
                LinkToFirstPicture = "manga/naruto/glava0/1.jpg",
                NumberOfPictures = 45
            }
        };

        return new MangaInput()
        {
            Name = "Naruto",
            PathToTitlePicture = "manga/naruto/titleimage.jpg",
            Genres_names = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Двенадцать лет назад, мощный Девятихвостый Демон-Лис напал на деревню ниндзя, Коноху. Демон был быстро побежден и запечатан в младенце по имени Наруто Узумаки. Но для этого, главному ниндзя Конохи, четвёртому хокаге пришлось пожертвовать жизнью... Теперь, по прошествии 12-и лет, Наруто является номером один среди придурков ниндзя, который полон решимости стать следующим Хокаге и получить признание всех, кто когда-либо сомневался в нем!",
            NumbetOfChapters = 702,
            AgeRating = "16+",
            Author = "KISHIMOTO Masashi",
            ReleaseYear = 1999
        };
    }
    private MangaInput CreateSevenDeadlySinsManga(IList<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Action",
           "Heroic fantasy",
           "Comedy",
           "Adventures",
           "Romance",
           "Supernatural",
           "Shounen",
           "Fantasy",
           "Angels",
           "Apocalypse",
           "Artifacts",
           "Gods",
           "GG man",
           "Demons",
           "Friendship",
           "Monsters",
           "Undead",
           "Knights",
           "Magic",
           "Elves",
           "Demon Lord",
           "Wizards / mages",
           "Reincarnation",
           "Middle Ages",
           "Empires",
           "Gg imba",
           "Amnesia / Memory Loss"
        };

        var genres_id = new List<string>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 0,
                LinkToFirstPicture = "manga/sevendeadlysins/glava0/1.jpg",
                NumberOfPictures = 52
            }
        };

        return new MangaInput()
        {
            Name = "Seven Deadly Sins",
            PathToTitlePicture = "manga/sevendeadlysins/titleimage.jpg",
            Genres_names = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "В королевстве Лионесс несколько рыцарей, прозванных «Семью смертными грехами» пыталисиь совершить государственный переворот. Им не позволили это сделать члены «Святого рыцарства». История на этом не завершилась и возобновилась спустя десять лет. Королевская семья была арестована, а сбежать удалось дочери короля Элизабет. Она полагает, что единственным шансом спастись выступают рыцари и, переодевшись так, чтобы её не узнали, отправилась искать Мелиодаса и его соратников. Она оказывается в таверне, не подозревая, что попала по назначению и отыскала рыцаря, на которого делала ставку. В Британии наконец-то настали спокойные дни, но опять-таки временно. Смельчакам рыцарям и Элизабет предстояло вовлечься в борьбу с Десятью заповедями. Весь мир оказался под серьезной угрозой. Печать вследствие происходящих событий была вскрыта, и демоны могли безо всяких препятствий покинули заточение, а в нем они провели века. Мерлин, Диана, Банд, Эсканор, Кинг и Хоук взялись за оружие и ступили на тропу войны, ведь интересы мира и королевства были превыше. После таких сражений рыцарям полагался отдых, но без новых приключений им не обойтись, а те будут ещё круче прежних.",
            NumbetOfChapters = 378,
            AgeRating = "16+",
            Author = "SUZUKI Nakaba",
            ReleaseYear = 2012
        };
    }
    private MangaInput CreateTokyoGhoulSinsManga(IList<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Action",
           "Psychology",
           "Supernatural",
           "Seinen",
           "Tragedy",
           "Horror",
           "Drama",
           "Romance",
           "Fantasy",
           "Antihero",
           "Survival",
           "Cruel world",
           "GG man",
           "Violence / cruelty",
           "Philosophy",
           "Japan",
           "Criminals / Crime",
           "Monsters",
           "Strength Ranks",
           "Sentient races",
           "Friendship",
           "Skills / abilities" 
        };

        var genres_id = new List<string>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/tokyoghoul/glava1/1.jpg",
                NumberOfPictures = 46
            }
        };

        return new MangaInput()
        {
            Name = "TokyoGhoul",
            PathToTitlePicture = "manga/tokyoghoul/titleimage.jpg",
            Genres_names = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Раса гулей существует с незапамятных времен. Её представители вовсе не против людей, они их даже любят — преимущественно в сыром виде. Любители человечины внешне неотличимы от нас, сильны, быстры и живучи — но их мало, потому гули выработали строгие правила охоты и маскировки, а нарушителей наказывают сами или по-тихому сдают борцам с нечистью. В век науки люди знают про гулей, но как говорится, привыкли. Власти не считают людоедов угрозой, более того, рассматривают их как идеальную основу для создания суперсолдат. Эксперименты идут уже давно…" +
            " Ничего этого не ведал Канэки Кэн, робкий и невзрачный токийский первокурсник, безнадежно влюбленный в красавицу-интеллектуалку Ризэ, частую гостью в кафе «Место встречи», где парень подрабатывает официантом. Не думал Кэн, что скоро самому придётся стать гулем, и многие знакомые предстанут в неожиданном свете. Главному герою предстоит мучительный поиск нового пути, ибо он понял, что люди и гули похожи: просто одни друг друга жрут в прямом смысле, другие — в переносном. Правда жизни жестока, переделать её нельзя, и силен тот, кто не отворачивается. А дальше уж как-нибудь!",
            NumbetOfChapters = 144,
            AgeRating = "18+",
            Author = "ISHIDA Sui",
            ReleaseYear = 2011
        };
    }
}
