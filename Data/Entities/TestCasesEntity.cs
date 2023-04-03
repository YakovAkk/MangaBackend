using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities
{
    public class TestCasesEntity
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get ; set ; }
        public int Order { get; set ; }
        public string TestCase { get; set; }

        public virtual TestEntity Test { get; set; }
    }
}
