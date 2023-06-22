using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace LeagueChampsDb.DataProcessor.ImportDto
{
    public class ImportAbilityDto
    {

        [Required]
        [JsonProperty("name")]
        public string Name { get; set; } = null!;

        [Required]
        [JsonProperty("champion")]
        public string Champion { get; set; } = null!;

        [Required]
        [JsonProperty("image_url")]
        public string ImageURL { get; set; } = null!;

        [Required]
        [JsonProperty("is_passive")]
        public bool IsPassive { get; set; }
    }
}
