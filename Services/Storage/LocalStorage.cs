using Microsoft.Extensions.Configuration;
using Services.Storage.Base;

namespace Services.Storage;

public class LocalStorage : ILocalStorage
{
    private readonly IConfiguration _configuration;
    public string RelativePath { get; init; }
    public LocalStorage(IConfiguration configuration )
    {
        var sectionsNameWithPath = "Others";
        var relativePathOnServer = "RelativePath";

        _configuration = configuration;
        RelativePath = _configuration.GetSection(sectionsNameWithPath)[relativePathOnServer];
    }
}
