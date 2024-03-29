﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Data.Entities;

public partial class GenreEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }
    public string Name { get; set; }
    public virtual List<MangaEntity> Mangas { get; set; }
    public GenreEntity()
    {
        Mangas = new List<MangaEntity>();
    }

    public override string ToString()
    {
        return $"Id = {Id} Name = {Name} Mangas = {Mangas}";
    }
}
