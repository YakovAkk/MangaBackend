using Services.DTO.Base;

namespace Services.DTO
{
    public class GenreDTO : IModelDTO
    {
        public string? Id { get; set; }
        public string Name { get ; set ; }
        public GenreDTO()
        {

        }

        public GenreDTO(string name)
        {
            Name = name;
        }
    }
}
