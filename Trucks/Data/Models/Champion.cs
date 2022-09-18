using LeagueChampsDb.Data.Models.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LeagueChampsDb.Data.Models
{
    public class Champion
    {
        public Champion()
        {
            this.UsersChampions = new HashSet<UserChampion>();
        }

        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Required, Url]
        public string ImageUrl { get; set; }

        [Required]
        public Position Position { get; set; }

        [Required]
        public PlaystyleClass Class { get; set; }

        [Required, Range(450, 6300)]
        public int PriceBE { get; set; }

        [Required, Range(260, 975)]
        public int PriceRP { get; set; }

        [Required]
        public DateTime ReleaseDate { get; set; }

        [Required]
        public bool HasMana { get; set; }

        [Required]
        public bool IsRanged { get; set; }

        public ICollection<UserChampion> UsersChampions { get; set; }
    }
}
