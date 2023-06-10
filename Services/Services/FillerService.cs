using Data.Database;
using Data.Entities;
using Microsoft.EntityFrameworkCore;
using Services.Model.InputModel;
using Services.Model.ViewModel;
using Services.Services.Base;

namespace Services.Services;

public class FillerService : DbService<AppDBContext>, IFillerService
{
    private readonly IMangaService _mangaService;
    private readonly IGenreService _genreService;
    private readonly IAuthService _authService;
    private readonly IUserService _userService;
    public FillerService(IMangaService mangaService, IGenreService genreService, IAuthService authService, 
        IUserService userService, DbContextOptions<AppDBContext> dbContextOptions
        ) : base(dbContextOptions)
    {
        _mangaService = mangaService;
        _genreService = genreService;
        _authService = authService;
        _userService = userService;
    }

    public async Task<ResponseViewModel> AddGenres()
    {
        var listOfGenres = new List<GenreInputModel>()
        {
            new GenreInputModel("Action"),
            new GenreInputModel("Romance"),
            new GenreInputModel("Comedy"),
            new GenreInputModel("Drama"),
            new GenreInputModel("Fantasy"),
            new GenreInputModel("Everyday life"),
            new GenreInputModel("Adventures"),
            new GenreInputModel("Art"),
            new GenreInputModel("Madness"),
            new GenreInputModel("Action movie"),
            new GenreInputModel("Military"),
            new GenreInputModel("Harem"),
            new GenreInputModel("Gender intrigue"),
            new GenreInputModel("Heroic fantasy"),
            new GenreInputModel("Demons"),
            new GenreInputModel("Detective"),
            new GenreInputModel("Children's"),
            new GenreInputModel("Josei"),
            new GenreInputModel("The game"),
            new GenreInputModel("Isekai"),
            new GenreInputModel("Story"),
            new GenreInputModel("Cyberpunk"),
            new GenreInputModel("Kodomo"),
            new GenreInputModel("Space"),
            new GenreInputModel("Magic"),
            new GenreInputModel("Maho-shoujo"),
            new GenreInputModel("Cars"),
            new GenreInputModel("Fur"),
            new GenreInputModel("Mystic"),
            new GenreInputModel("Music"),
            new GenreInputModel("Science fiction"),
            new GenreInputModel("Omegaverse"),
            new GenreInputModel("Parody"),
            new GenreInputModel("Police"),
            new GenreInputModel("Post-apocalyptic"),
            new GenreInputModel("Psychology"),
            new GenreInputModel("Samurai fighter"),
            new GenreInputModel("Supernatural"),
            new GenreInputModel("Shojo"),
            new GenreInputModel("Shojo-ai"),
            new GenreInputModel("Shonen"),
            new GenreInputModel("Shonen Ai"),
            new GenreInputModel("Sport"),
            new GenreInputModel("Superpower"),
            new GenreInputModel("Seinen"),
            new GenreInputModel("Tragedy"),
            new GenreInputModel("Thriller"),
            new GenreInputModel("Horror"),
            new GenreInputModel("Fiction"),
            new GenreInputModel("Schlola"),
            new GenreInputModel("Erotica"),
            new GenreInputModel("Ecchi"),
            new GenreInputModel("Yuri"),
            new GenreInputModel("Yaoi"),
            new GenreInputModel("Burlesque"),
            new GenreInputModel("Travesty"),
            new GenreInputModel("Poem"),
        };

        try
        {
            var resultGenre = await _genreService.AddRangeAsync(listOfGenres);

            if (!resultGenre.Any())
            {
                return new ResponseViewModel()
                {
                    IsSuccess = false,
                    MessageWhatWrong = "The Ganres is already conteited in the database"
                };
            }

            return new ResponseViewModel()
            {
                IsSuccess = true,
                MessageWhatWrong = ""
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel()
            {
                IsSuccess = false,
                MessageWhatWrong = ex.Message
            };
        }
    }
    public async Task<ResponseViewModel> AddMangas()
    {
        var genres = await _genreService.GetAllAsync();

        if (!genres.Any())
        {
            return new ResponseViewModel()
            {
                IsSuccess = false,
                MessageWhatWrong = "The database doesn't have any genres!"
            };
        }

        var mangas = new List<MangaInputModel>();
        mangas.Add(CreateAttackOfTheTitansManga(genres));
        mangas.Add(CreateNarutoManga(genres));
        mangas.Add(CreateSevenDeadlySinsManga(genres));
        mangas.Add(CreateTokyoGhoulSinsManga(genres));
        mangas.Add(CreatChornaRadaItem(genres));
        mangas.Add(CreateInstytutkaItem(genres));
        mangas.Add(CreateIntermezzoItem(genres));
        mangas.Add(CreateZaDvomaZaizamiItem(genres));
        mangas.Add(CreatIaRomantikaItem(genres));
        mangas.Add(CreatMistoItem(genres));
        mangas.Add(CreatMasterOfShipItem(genres));
        mangas.Add(CreateEneidaItem(genres));
        mangas.Add(CreateTyhrolovyItem(genres));
        mangas.Add(CreateTiniPredkivItem(genres));
        mangas.Add(CreateZhovtyKniazItem(genres));
        mangas.Add(CreatMarusiaOfShipItem(genres));
        mangas.Add(CreatKhibaRevytVoluItem(genres));
        mangas.Add(CreatKaydashevaSimiaItem(genres));
        mangas.Add(CreatMarusiaChurayItem(genres));

        try
        {
            var resultManga = await _mangaService.AddRangeAsync(mangas);

            if (!resultManga.Any())
            {
                return new ResponseViewModel()
                {
                    IsSuccess = false,
                    MessageWhatWrong = "The mangas is already conteined in the database!"
                };
            }

            return new ResponseViewModel()
            {
                IsSuccess = true,
                MessageWhatWrong = ""
            };
        }
        catch (Exception ex)
        {
            return new ResponseViewModel()
            {
                IsSuccess = false,
                MessageWhatWrong = ex.Message
            };
        }
    }
    public async Task<ResponseViewModel> AddAdmin()
    {
        var admin = new UserRegistrationInputModel()
        {
            Name = "admin",
            Email = "admin@gmail.com",
            Password = "pa$$w0rd1",
            ConfirmPassword = "pa$$w0rd1"
        };

        var response = await _authService.RegisterAsync(admin);

        if (response is null)
        {
            return new ResponseViewModel()
            {
                IsSuccess = false,
                MessageWhatWrong = "Error"
            };
        }

        return new ResponseViewModel()
        {
            IsSuccess = true,
            MessageWhatWrong = ""
        };
    }
    public async Task<ResponseViewModel> DeleteUser(UserInputModel userInputModel)
    {
        var user = await _userService.GetUserByNameAsync(userInputModel.Name);
        if (user == null)
            throw new Exception("User doesn't exist!");

        using(var dbContext = CreateDbContext())
        {
            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();
        }

        return new ResponseViewModel
        {
            IsSuccess = true,
            MessageWhatWrong = ""
        };
    }

    #region Private
    private MangaInputModel CreateAttackOfTheTitansManga(IList<GenreEntity> genres)
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

        var genres_id = new List<int>();
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

        return new MangaInputModel()
        {
            Name = "Attack of the Titans",
            PathToTitlePicture = "manga/attackofthetitans/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Давным-давно человечество было всего лишь «их» кормом, до тех пор, пока оно не построило гигантскую стену вокруг своей страны. С тех пор прошло сто лет мира и большинство людей жили счастливой, беззаботной жизнью. Но за долгие годы спокойствия пришлось заплатить огромную цену, и в 845 году они снова познали чувство ужаса и беспомощности – стена, которая была их единственным спасением, пала. «Они» прорвались. Половина человечества съедена, треть территории навсегда потеряна...",
            NumbetOfChapters = 140,
            AgeRating = "18+",
            Author = "ISAYAMA Hajime",
            ReleaseYear = 2009
        };
    }
    private MangaInputModel CreateNarutoManga(List<GenreEntity> genres)
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

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/naruto/glava1/1.jpg",
                NumberOfPictures = 45
            }
        };

        return new MangaInputModel()
        {
            Name = "Naruto",
            PathToTitlePicture = "manga/naruto/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Двенадцать лет назад, мощный Девятихвостый Демон-Лис напал на деревню ниндзя, Коноху. Демон был быстро побежден и запечатан в младенце по имени Наруто Узумаки. Но для этого, главному ниндзя Конохи, четвёртому хокаге пришлось пожертвовать жизнью... Теперь, по прошествии 12-и лет, Наруто является номером один среди придурков ниндзя, который полон решимости стать следующим Хокаге и получить признание всех, кто когда-либо сомневался в нем!",
            NumbetOfChapters = 702,
            AgeRating = "16+",
            Author = "KISHIMOTO Masashi",
            ReleaseYear = 1999
        };
    }
    private MangaInputModel CreateSevenDeadlySinsManga(List<GenreEntity> genres)
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

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/sevendeadlysins/glava1/1.jpg",
                NumberOfPictures = 52
            }
        };

        return new MangaInputModel()
        {
            Name = "Seven Deadly Sins",
            PathToTitlePicture = "manga/sevendeadlysins/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "В королевстве Лионесс несколько рыцарей, прозванных «Семью смертными грехами» пыталисиь совершить государственный переворот. Им не позволили это сделать члены «Святого рыцарства». История на этом не завершилась и возобновилась спустя десять лет. Королевская семья была арестована, а сбежать удалось дочери короля Элизабет. Она полагает, что единственным шансом спастись выступают рыцари и, переодевшись так, чтобы её не узнали, отправилась искать Мелиодаса и его соратников. Она оказывается в таверне, не подозревая, что попала по назначению и отыскала рыцаря, на которого делала ставку. В Британии наконец-то настали спокойные дни, но опять-таки временно. Смельчакам рыцарям и Элизабет предстояло вовлечься в борьбу с Десятью заповедями. Весь мир оказался под серьезной угрозой. Печать вследствие происходящих событий была вскрыта, и демоны могли безо всяких препятствий покинули заточение, а в нем они провели века. Мерлин, Диана, Банд, Эсканор, Кинг и Хоук взялись за оружие и ступили на тропу войны, ведь интересы мира и королевства были превыше. После таких сражений рыцарям полагался отдых, но без новых приключений им не обойтись, а те будут ещё круче прежних.",
            NumbetOfChapters = 378,
            AgeRating = "16+",
            Author = "SUZUKI Nakaba",
            ReleaseYear = 2012
        };
    }
    private MangaInputModel CreateTokyoGhoulSinsManga(List<GenreEntity> genres)
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

        var genres_id = new List<int>();
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

        return new MangaInputModel()
        {
            Name = "TokyoGhoul",
            PathToTitlePicture = "manga/tokyoghoul/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Раса гулей существует с незапамятных времен. Её представители вовсе не против людей, они их даже любят — преимущественно в сыром виде. Любители человечины внешне неотличимы от нас, сильны, быстры и живучи — но их мало, потому гули выработали строгие правила охоты и маскировки, а нарушителей наказывают сами или по-тихому сдают борцам с нечистью. В век науки люди знают про гулей, но как говорится, привыкли. Власти не считают людоедов угрозой, более того, рассматривают их как идеальную основу для создания суперсолдат. Эксперименты идут уже давно…" +
            " Ничего этого не ведал Канэки Кэн, робкий и невзрачный токийский первокурсник, безнадежно влюбленный в красавицу-интеллектуалку Ризэ, частую гостью в кафе «Место встречи», где парень подрабатывает официантом. Не думал Кэн, что скоро самому придётся стать гулем, и многие знакомые предстанут в неожиданном свете. Главному герою предстоит мучительный поиск нового пути, ибо он понял, что люди и гули похожи: просто одни друг друга жрут в прямом смысле, другие — в переносном. Правда жизни жестока, переделать её нельзя, и силен тот, кто не отворачивается. А дальше уж как-нибудь!",
            NumbetOfChapters = 144,
            AgeRating = "18+",
            Author = "ISHIDA Sui",
            ReleaseYear = 2011
        };
    }
    private MangaInputModel CreateEneidaItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Action",
           "Martial arts",
           "Drama",
           "Comedy",
           "Adventures",
           "Shounen",
           "Sword fighting",
           "War",
           "GG man",
           "Friendship",
           "Revenge",
           "Poem",
           "Burlesque",
           "Travesty"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/eneida/glava1/eneida.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Eneida",
            PathToTitlePicture = "manga/eneida/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Українська бурлескно-травестійна поема, написана українським письменником Іваном Котляревським, заснована на сюжеті однойменної класичної поеми римського поета Вергілія. Складається з шести частин, на відміну від дванадцяти частин Вергілія. Написана чотиристопним ямбом.\r\n\r\nПоема написана в добу становлення романтизму і націоналізму в Європі, на тлі ностальгії частини української еліти за козацькою державою, ліквідованою Росією в 1775—1786 роках. «Енеїда» — перша масштабна пам'ятка українського письменства, укладена розмовною українською мовою. Поема започаткувала становлення новочасної української літератури. Перші три частини поеми були видані в 1798 році, в Санкт-Петербурзі, без відома автора, під назвою: «Енеида. На малороссійскій языкъ перелиціованная И. Котляревскимъ». Повністю «Енеїда» вийшла в світ після смерті Котляревського, в 1842 році. Цей твір є першокласним джерелом з українознавства, українського побуту та культури XVIII століття.\r\n\r\nЯк і оригінал, сюжет описує пригоди троянського отамана Енея, проте у викладі Котляревського вони подаються в антуражі тогочасної української культури. Після зруйнування батьківщини ворогами, Еней разом зі своїм козацьким військом шукає місця, де зміг би заснувати майбутню імперію. У його поневіряння втручаються боги, намагаючись хто допомогти, а хто завадити його подорожі.",
            NumbetOfChapters = 12,
            AgeRating = "0+",
            Author = "Ivan Petrovich Kotlyarevskyi",
            ReleaseYear = 1798
        };
    }
    private MangaInputModel CreateIntermezzoItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Martial arts",
           "Drama"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/intermezzo/glava1/intermezzo.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Intermezzo",
            PathToTitlePicture = "manga/intermezzo/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "імпресіоністична новела українського письменника Михайла Коцюбинського, написана в 1908 році в Чернігові. Новела складається з 11 частин, що пов’язані образом митця — учасника й оповідача зображуваного; розповідає про його духовне одужання при зустрічі з природою. У творі порушено проблеми душевної рівноваги, повноцінного життя, специфіки творчого процесу.",
            NumbetOfChapters = 12,
            AgeRating = "0+",
            Author = "Mykhailo Mykhailovych Kotsyubynskyi",
            ReleaseYear = 1908
        };
    }
    private MangaInputModel CreateInstytutkaItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Poem"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/instytutka/glava1/instytutka.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Інститутка",
            PathToTitlePicture = "manga/instytutka/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "повість української письменниці Марко Вовчок, вперше видана у 1862 році в українському діаспорному журналі Санкт-Петербургу «Основа». Перша в українській літературі соціальна повість.\r\nВ основі сюжету твору — наростання соціальних суперечностей в українському селі напередодні ліквідації кріпацтва, розкриття характерів двох антагоністичних сил: панів-кріпосників і селян-кріпаків, стихійний протест проти жорстокості та сваволі панства, засудження кріпацтва як великого соціального зла.",
            NumbetOfChapters = 12,
            AgeRating = "18+",
            Author = "Marko Vovchok",
            ReleaseYear = 1862
        };
    }
    private MangaInputModel CreateZaDvomaZaizamiItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Poem"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/za-dvoma-zaytsiamy/glava1/za-dvoma-zaytsiamy.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "За двома зайцями",
            PathToTitlePicture = "manga/za-dvoma-zaytsiamy/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "комедійна п'єса українського драматурга Михайла Старицького. Написана 1883 року українською мовою.\r\nУ ній розповідається про цирульника Свирида Голохвостого, який намагається розбагатіти, одружившись із багатою міщанкою Пронею Сірко, і, водночас, залицяється до бідної дівчини-красуні Галі.\r\nУ п'єсі порушується проблема соціальної нерівності, висміюється життя українських русифікованих міщан Києва.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Mykhailo Petrovich Starytskyi",
            ReleaseYear = 1883
        };
    }
    private MangaInputModel CreateTyhrolovyItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Action",
           "Martial arts",
           "Drama",
           "Comedy",
           "Adventures",
           "GG man",
           "Friendship",
           "Revenge",
           "Poem"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/tyhrolovy/glava1/tyhrolovy.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Тигролови",
            PathToTitlePicture = "manga/tyhrolovy/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Пригодницький роман з автобіографічними елементами Івана Багряного, написаний та виданий 1944 року як «Звіролови» у львівському журналі «Вечірня година». Чернетка оригіналу тексту «Звіроловів/Тигроловів» залишилась в підрадянській Україні й відповідно після переїзду автора в Німеччину у 1944—1946 роках Багряному довелося повністю відновлювати текст з пам'яті; цю відновлену версію було видано 1946 року під назвою «Тигролови» у ной-ульмському видавництві «Прометей».",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Ivan Bagryany",
            ReleaseYear = 1946
        };
    }
    private MangaInputModel CreateTiniPredkivItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Poem"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/tini-zabutykh-predkiv/glava1/tini-zabutykh-predkiv.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Тіні забутих предків",
            PathToTitlePicture = "manga/tini-zabutykh-predkiv/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "повість українського письменника Михайла Коцюбинського, написана під враженням його перебування на Гуцульщині в 1911 році.\r\nУ творі розповідається про кохання гуцулів Івана й Марічки з ворогуючих родів. У повісті яскраво передано гуцульські побут і життя з елементами фольклору.",
            NumbetOfChapters = 12,
            AgeRating = "12+",
            Author = "Mykhailo Mykhailovych Kotsiubynskyi",
            ReleaseYear = 1912
        };
    }
    private MangaInputModel CreateZhovtyKniazItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Poem"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/zhovtyy-kniaz/glava1/zhovtyy-kniaz.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Жовтий князь",
            PathToTitlePicture = "manga/zhovtyy-kniaz/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "роман українського письменника Василя Барки, присвячений Голодомору 1932—1933 років.",
            NumbetOfChapters = 12,
            AgeRating = "18+",
            Author = "Vasyl Barka",
            ReleaseYear = 1962
        };
    }
    private MangaInputModel CreatChornaRadaItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/chorna-rada/glava1/chorna-rada.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Чорна рада",
            PathToTitlePicture = "manga/chorna-rada/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Історичний роман українського письменника Пантелеймона Куліша, у якому зображено відому історичну подію — чорну раду, що відбулася в Ніжині 1663 року, відтворено соціальні суперечності в Україні після переможної визвольної війни та приєднання до Московського царства.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Panteleimon Oleksandrovich Kulish",
            ReleaseYear = 1857
        };
    }
    private MangaInputModel CreatMistoItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/misto/glava1/misto.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Місто",
            PathToTitlePicture = "manga/misto/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Урбаністичний роман українського письменника Валер'яна Підмогильного, опублікований 1928 року.\r\nВалер'ян Підмогильний створив модерний роман, в якому, на відміну від традиційної селянської і соціальної тематики, акцент перенесений на урбаністичну проблематику, порушені філософські питання буття й аналізована психіка героїв, а конфлікт розгортається між людьми з різними світоглядами. «Місто» — перший урбаністичний роман в українській літературі, з новими героями, проблематикою та манерою оповіді.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Valerian Petrovych Podoghilnyi",
            ReleaseYear = 1928
        };
    }
    private MangaInputModel CreatMasterOfShipItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/mayster-korablia/glava1/mayster-korablia.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Майстер корабля",
            PathToTitlePicture = "manga/mayster-korablia/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Дебютний роман українського письменника Юрія Яновського, що порушує низку питань, особливо актуальних для модерністського покоління митців, та відображає творчу атмосферу 20–х років XX століття. Текст має ознаки містифікації, але все ж уважається автобіографічним. Роман написаний у 1928 році.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Yury Ivanovich Yanovskyi",
            ReleaseYear = 1928
        };
    }
    private MangaInputModel CreatMarusiaOfShipItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance",
           "Drama"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/marusia/glava1/marusia.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Маруся",
            PathToTitlePicture = "manga/marusia/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Драматична повість Григорія Квітки-Основ'яненка, написана в 1832 та опублікована в 1834 році.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Grigory Fedorovich Kvitka-Osnovyanenko",
            ReleaseYear = 1834
        };
    }
    private MangaInputModel CreatIaRomantikaItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/ia-(romantyka)/glava1/ia-(romantyka).pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Я (Романтика)",
            PathToTitlePicture = "manga/ia-(romantyka)/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Психологічна новела українського письменника Миколи Хвильового, ідеєю якої є фатальна невідповідність між ідеалами революції та методами їх досягнення, засудження більшовицького революційного фанатизму; світ врятує любов, усепрощення.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Mykola Khvylovy",
            ReleaseYear = 1924
        };
    }
    private MangaInputModel CreatKhibaRevytVoluItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/khiba-revut-voly-iak-iasla-povni/glava1/khiba-revut-voly-iak-iasla-povni.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Хіба ревуть воли, як ясла повні?",
            PathToTitlePicture = "manga/khiba-revut-voly-iak-iasla-povni/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Роман українського письменника Панаса Мирного, написаний у співавторстві з Іваном Біликом, Створювався 4 роки (1872-1875). У листопаді 1872 р. була написана повість «Чіпка». Остаточний варіант твору під назвою «Пропаща сила» або «Хіба ревуть воли, як ясла повні» був опублікований 1880 року в Женеві.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Panas Mirnyi",
            ReleaseYear = 1875
        };
    }
    private MangaInputModel CreatKaydashevaSimiaItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/kaydasheva-simia/glava1/kaydasheva-simia.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Кайдашева сім'я",
            PathToTitlePicture = "manga/kaydasheva-simia/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Реалістична соціально-побутова повість українського письменника Івана Семеновича Нечуя-Левицького, написана 1878 року та вперше надрукована 1879 року у львівському журналі «Правда».\r\nУ повісті через серію трагікомічних ситуацій з життя родини Кайдашів демонструється шкода від духовної роз'єднаності, яка призводить до егоїзму, розбрату, невмілого користування спадком попередніх поколінь.",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Nechuy-Levytsky Ivan Semenovych",
            ReleaseYear = 1879
        };
    }
    private MangaInputModel CreatMarusiaChurayItem(List<GenreEntity> genres)
    {
        var genresForTheManga = new List<string>()
        {
           "Romance"
        };

        var genres_id = new List<int>();
        foreach (var genre in genres.Where(i => genresForTheManga.Contains(i.Name)))
        {
            genres_id.Add(genre.Id);
        }

        var PathToFoldersWithGlava = new List<GlavaMangaEntity>()
        {
            new GlavaMangaEntity()
            {
                NumberOfGlava = 1,
                LinkToFirstPicture = "manga/marusia-churay/glava1/marusia-churay.pdf",
                NumberOfPictures = 1
            }
        };

        return new MangaInputModel()
        {
            Name = "Маруся Чурай",
            PathToTitlePicture = "manga/marusia-churay/titleimage.jpg",
            Genres_Ids = genres_id,
            PathToFoldersWithGlava = PathToFoldersWithGlava,
            Description = "Історичний роман у віршах української письменниці Ліни Костенко, опублікований 1979 року. Сюжет вибудовано навколо легенди про Марусю Чурай — відому українську піснярку. У міні-пролозі роману є вказівка на історичну основу твору: «Влітку 1658 року Полтава згоріла дощенту»[1]. Справжні події XVII ст., на тлі яких розвивається сюжет твору, відтворено в образах Богдана Хмельницького, Якова Остряниці, Северина Наливайка, Павла Павлюка, Яреми Вишневецького. У 1987 році за роман авторка була відзначена премією імені Тараса Шевченка[2].",
            NumbetOfChapters = 12,
            AgeRating = "16+",
            Author = "Nechuy-Levytsky Ivan Semenovych",
            ReleaseYear = 1979
        };
    }

 
    #endregion
}
