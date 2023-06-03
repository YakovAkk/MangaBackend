namespace Services.Model.DTO;

public class GenreInput 
{
    public string Name { get; set; }
    public GenreInput(string name)
    {
        Name = name;
    }
}
