namespace Services.Model.InputModel;

public class GenreInputModel
{
    public string Name { get; set; }
    public GenreInputModel(string name)
    {
        Name = name;
    }
}
