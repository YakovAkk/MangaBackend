using Services.Model.DTO.Base;

namespace Services.Model.DTO;

public class GenreInput : IModelInput
{
    public int Id { get; set; }
    public string Name { get; set; }
    public GenreInput(string name)
    {
        Name = name;
    }
}
