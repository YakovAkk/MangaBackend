using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Data.Entities
{
    public class RememberReadingItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public UserEntity User { get; set; }
        public int MangaId { get; set; }
        public int ChapterNumber { get; set; }
        public int Page { get; set; }
    }
}
