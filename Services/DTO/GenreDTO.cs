using Services.DTO.Base;

namespace Services.DTO;

public class GenreDTO : IModelDTO
{
    public string? Id { get; set; }
    public string Name { get ; set ; }

    public override string ToString()
    {
        return $"Id = {Id} Name = {Name}";
    }

    public GenreDTO()
    {

    }

    public GenreDTO(string name)
    {
        Name = name;
    }
}
