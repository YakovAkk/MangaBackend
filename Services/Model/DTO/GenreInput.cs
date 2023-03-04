using Services.Model.DTO.Base;

namespace Services.Model.DTO;

public class GenreInput : IModelInput
{
    public string? Id { get; set; }
    public string Name { get; set; }

    public override string ToString()
    {
        return $"Id = {Id} Name = {Name}";
    }

    public GenreInput()
    {

    }

    public GenreInput(string name)
    {
        Name = name;
    }
}
