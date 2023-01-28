using Services.Model.DTO.Base;

namespace Services.Model.DTO;

public class GenreDTO : IModelDTO
{
    public string? Id { get; set; }
    public string Name { get; set; }

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
