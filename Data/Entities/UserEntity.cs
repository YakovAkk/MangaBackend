﻿using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Data.Entities.Base;

namespace Data.Entities;
public class UserEntity : IEntity
{
    [Key]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public string? Id { get; set; }
    public string Name { get; set; }
    public string DeviceToken { get; set; }
    public virtual List<MangaEntity> FavoriteMangas { get; set; }
    public virtual List<GenreEntity> FavoriteGenres { get; set; }

    public UserEntity()
    {
        FavoriteMangas = new List<MangaEntity>();
        FavoriteGenres= new List<GenreEntity>();
    }

    public override string ToString()
    {
        return $"Id = {Id} Name = {Name} DeviceToken = {DeviceToken}";
    }
}