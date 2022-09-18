using System;
using System.ComponentModel.DataAnnotations;

namespace LeagueChampsDb.DataProcessor.ImportDto
{
    public class ImportChampionDto
    {
        [Required]
        public string Name { get; set; }

        [MaxLength(50)]
        public string Description { get; set; }

        [Required, Url]
        public string Image { get; set; }

        [Required]
        public string Position { get; set; }

        [Required]
        public string Class { get; set; }

        [Required, Range(450, 6300)]
        public int PriceBE { get; set; }

        [Required, Range(260, 975)]
        public int PriceRP { get; set; }

        [Required]
        public string ReleaseDate { get; set; }

        [Required]
        public bool HasMana { get; set; }

        [Required]
        public bool IsRanged { get; set; }
    }
}
